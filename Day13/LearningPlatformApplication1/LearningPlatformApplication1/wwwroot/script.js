const $ = id => document.getElementById(id);

let allCourses = [];
let editingCourseCode = null;
let editingLearnerId = null;

function escapeHtml(value) {
    return String(value ?? "").replace(/[&<>"']/g, character => ({
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': "&quot;",
        "'": "&#39;"
    })[character]);
}

async function api(url, options = {}) {
    const response = await fetch(url, {
        ...options,
        headers: {
            "Content-Type": "application/json",
            ...options.headers
        }
    });

    const text = await response.text();
    let data = {};

    if (text) {
        try {
            data = JSON.parse(text);
        } catch {
            data = { message: text };
        }
    }

    if (!response.ok) {
        throw new Error(
            data.message || `Request failed (${response.status})`
        );
    }

    return data;
}

function showMessage(id, message, success = true) {
    const element = $(id);
    element.textContent = message;
    element.className = success ? "message success" : "message error";
}

function formatMoney(value) {
    return new Intl.NumberFormat("en-IN", {
        style: "currency",
        currency: "INR",
        maximumFractionDigits: 2
    }).format(value);
}

function showError(id, error) {
    showMessage(id, error.message || "Something went wrong.", false);
}


// ---------------- COURSES ----------------

async function loadCourses() {
    try {
        allCourses = await api("/api/courses");
        renderCourseTable(allCourses);
        renderCourseOptions();
    } catch (error) {
        showMessage("courseMessage", error.message, false);
    }
}

function renderCourseTable(courses) {
    $("courseTable").innerHTML = courses.map(course => `
        <tr>
            <td>${escapeHtml(course.courseCode)}</td>
            <td>${escapeHtml(course.title)}</td>
            <td>${formatMoney(course.fee)}</td>
            <td>${course.durationDays} days</td>
            <td>${course.availableSeatCount}</td>
            <td>
                <button class="small-button"
                    data-action="edit-course"
                    data-id="${escapeHtml(course.courseCode)}">
                    Edit
                </button>
                <button class="small-button danger"
                    data-action="delete-course"
                    data-id="${escapeHtml(course.courseCode)}">
                    Delete
                </button>
            </td>
        </tr>
    `).join("");

    if (courses.length === 0) {
        $("courseTable").innerHTML =
            '<tr><td colspan="6">No courses found.</td></tr>';
    }
}

function renderCourseOptions() {
    $("courseCodeSelect").innerHTML =
        '<option value="">Select a course</option>' +
        allCourses.map(course => `
            <option value="${escapeHtml(course.courseCode)}">
                ${escapeHtml(course.title)}
                (${course.availableSeatCount} seats available)
            </option>
        `).join("");
}

async function searchCourses() {
    try {
        const params = new URLSearchParams();
        const search = $("courseSearch").value.trim();
        const sort = $("sortOrder").value;

        if (search) params.set("search", search);
        if (sort) params.set("sort", sort);

        const query = params.toString();
        const courses = await api(
            "/api/courses" + (query ? `?${query}` : "")
        );

        renderCourseTable(courses);
    } catch (error) {
        showError("courseMessage", error);
    }
}

$("searchCoursesBtn").addEventListener("click", searchCourses);
$("sortOrder").addEventListener("change", searchCourses);

$("resetCoursesBtn").addEventListener("click", async () => {
    $("courseSearch").value = "";
    $("sortOrder").value = "";
    await loadCourses();
});

$("courseSearch").addEventListener("keydown", event => {
    if (event.key === "Enter") {
        event.preventDefault();
        searchCourses();
    }
});

$("courseForm").addEventListener("submit", async event => {
    event.preventDefault();

    const course = {
        courseCode: $("courseCode").value.trim(),
        title: $("courseTitle").value.trim(),
        fee: Number($("courseFee").value),
        durationDays: Number($("courseDuration").value),
        totalSeats: Number($("courseSeats").value)
    };

    try {
        const url = editingCourseCode
            ? `/api/courses/${encodeURIComponent(editingCourseCode)}`
            : "/api/courses";

        const method = editingCourseCode ? "PUT" : "POST";

        const result = await api(url, {
            method,
            body: JSON.stringify(course)
        });

        showMessage("courseMessage", result.message);
        resetCourseForm();
        await loadCourses();
    } catch (error) {
        showError("courseMessage", error);
    }
});

async function editCourse(code) {
    try {
        const course = await api(
            `/api/courses/${encodeURIComponent(code)}`
        );

        editingCourseCode = course.courseCode;

        $("courseCode").value = course.courseCode;
        $("courseCode").disabled = true;
        $("courseTitle").value = course.title;
        $("courseFee").value = course.fee;
        $("courseDuration").value = course.durationDays;
        $("courseSeats").value = course.totalSeats;

        $("courseFormTitle").textContent = "Edit Course";
        $("courseSubmit").textContent = "Save Changes";
        $("courseCancelEdit").hidden = false;

        $("courseForm").scrollIntoView({ behavior: "smooth" });
    } catch (error) {
        showError("courseMessage", error);
    }
}

function resetCourseForm() {
    $("courseForm").reset();
    $("courseCode").disabled = false;
    editingCourseCode = null;

    $("courseFormTitle").textContent = "Add Course";
    $("courseSubmit").textContent = "Add Course";
    $("courseCancelEdit").hidden = true;
}

$("courseCancelEdit").addEventListener("click", resetCourseForm);

async function deleteCourse(code) {
    if (!confirm(`Delete course ${code}?`)) return;

    try {
        const result = await api(
            `/api/courses/${encodeURIComponent(code)}`,
            { method: "DELETE" }
        );

        showMessage("courseMessage", result.message);
        await loadCourses();
    } catch (error) {
        showError("courseMessage", error);
    }
}

$("courseTable").addEventListener("click", event => {
    const button = event.target.closest("button[data-action]");
    if (!button) return;

    const code = button.dataset.id;

    if (button.dataset.action === "edit-course") {
        editCourse(code);
    } else if (button.dataset.action === "delete-course") {
        deleteCourse(code);
    }
});


// ---------------- LEARNERS ----------------

async function loadLearners() {
    try {
        const learners = await api("/api/learners");

        $("learnerTable").innerHTML = learners.map(learner => `
            <tr>
                <td>${escapeHtml(learner.learnerId)}</td>
                <td>${escapeHtml(learner.name)}</td>
                <td>${escapeHtml(learner.contactNumber)}</td>
                <td>${escapeHtml(learner.email)}</td>
                <td>
                    <button class="small-button"
                        data-action="edit-learner"
                        data-id="${escapeHtml(learner.learnerId)}">
                        Edit
                    </button>
                    <button class="small-button danger"
                        data-action="delete-learner"
                        data-id="${escapeHtml(learner.learnerId)}">
                        Delete
                    </button>
                </td>
            </tr>
        `).join("");

        if (learners.length === 0) {
            $("learnerTable").innerHTML =
                '<tr><td colspan="5">No learners registered.</td></tr>';
        }
    } catch (error) {
        showError("learnerMessage", error);
    }
}

$("learnerForm").addEventListener("submit", async event => {
    event.preventDefault();

    const learner = {
        learnerId: $("learnerId").value.trim(),
        name: $("learnerName").value.trim(),
        contactNumber: $("contactNumber").value.trim(),
        email: $("email").value.trim()
    };

    try {
        const url = editingLearnerId
            ? `/api/learners/${encodeURIComponent(editingLearnerId)}`
            : "/api/learners";

        const method = editingLearnerId ? "PUT" : "POST";

        const result = await api(url, {
            method,
            body: JSON.stringify(learner)
        });

        showMessage("learnerMessage", result.message);
        resetLearnerForm();
        await loadLearners();
    } catch (error) {
        showError("learnerMessage", error);
    }
});

async function editLearner(id) {
    try {
        const learner = await api(
            `/api/learners/${encodeURIComponent(id)}`
        );

        editingLearnerId = learner.learnerId;

        $("learnerId").value = learner.learnerId;
        $("learnerId").disabled = true;
        $("learnerName").value = learner.name;
        $("contactNumber").value = learner.contactNumber;
        $("email").value = learner.email;

        $("learnerFormTitle").textContent = "Edit Learner";
        $("learnerSubmit").textContent = "Save Changes";
        $("learnerCancelEdit").hidden = false;

        $("learnerForm").scrollIntoView({ behavior: "smooth" });
    } catch (error) {
        showError("learnerMessage", error);
    }
}

function resetLearnerForm() {
    $("learnerForm").reset();
    $("learnerId").disabled = false;
    editingLearnerId = null;

    $("learnerFormTitle").textContent = "Register Learner";
    $("learnerSubmit").textContent = "Register Learner";
    $("learnerCancelEdit").hidden = true;
}

$("learnerCancelEdit").addEventListener("click", resetLearnerForm);

async function deleteLearner(id) {
    if (!confirm(`Delete learner ${id}?`)) return;

    try {
        const result = await api(
            `/api/learners/${encodeURIComponent(id)}`,
            { method: "DELETE" }
        );

        showMessage("learnerMessage", result.message);
        await loadLearners();
    } catch (error) {
        showError("learnerMessage", error);
    }
}

$("learnerTable").addEventListener("click", event => {
    const button = event.target.closest("button[data-action]");
    if (!button) return;

    const id = button.dataset.id;

    if (button.dataset.action === "edit-learner") {
        editLearner(id);
    } else if (button.dataset.action === "delete-learner") {
        deleteLearner(id);
    }
});


// ---------------- ENROLLMENTS ----------------

$("enrollmentForm").addEventListener("submit", async event => {
    event.preventDefault();

    const enrollment = {
        learnerId: $("enrollLearnerId").value.trim(),
        courseCode: $("courseCodeSelect").value
    };

    try {
        const result = await api("/api/enrollments", {
            method: "POST",
            body: JSON.stringify(enrollment)
        });

        showMessage("enrollmentMessage", result.message);
        $("enrollmentForm").reset();

        await loadCourses();
        await loadEnrollments();
    } catch (error) {
        showError("enrollmentMessage", error);
    }
});

async function loadEnrollments() {
    try {
        const enrollments = await api("/api/enrollments");
        const statusNames = ["Active", "Cancelled", "Completed"];

        $("enrollmentTable").innerHTML = enrollments.map(enrollment => {
            const status = typeof enrollment.status === "number"
                ? statusNames[enrollment.status]
                : enrollment.status;

            return `
                <tr>
                    <td>${enrollment.enrollmentId}</td>
                    <td>${escapeHtml(enrollment.learnerId)}</td>
                    <td>${escapeHtml(enrollment.courseCode)}</td>
                    <td>${formatMoney(enrollment.baseFee)}</td>
                    <td>${formatMoney(enrollment.discountAmount)}</td>
                    <td>${formatMoney(enrollment.finalFee)}</td>
                    <td>${escapeHtml(status)}</td>
                    <td>
                        ${status === "Active"
                            ? `<button class="small-button danger"
                                data-action="cancel-enrollment"
                                data-id="${enrollment.enrollmentId}">
                                Cancel
                               </button>`
                            : "-"}
                    </td>
                </tr>`;
        }).join("");

        if (enrollments.length === 0) {
            $("enrollmentTable").innerHTML =
                '<tr><td colspan="8">No enrollment records.</td></tr>';
        }
    } catch (error) {
        showError("enrollmentMessage", error);
    }
}

async function cancelEnrollment(id) {
    if (!confirm(`Cancel enrollment ${id}?`)) return;

    try {
        const result = await api(
            `/api/enrollments/${id}/cancel`,
            { method: "POST" }
        );

        showMessage("enrollmentMessage", result.message);

        await loadCourses();
        await loadEnrollments();
    } catch (error) {
        showError("enrollmentMessage", error);
    }
}

$("enrollmentTable").addEventListener("click", event => {
    const button = event.target.closest(
        'button[data-action="cancel-enrollment"]'
    );

    if (button) {
        cancelEnrollment(Number(button.dataset.id));
    }
});


// ---------------- START APPLICATION ----------------

async function initializeApp() {
    await loadCourses();
    await loadLearners();
    await loadEnrollments();
}

initializeApp();