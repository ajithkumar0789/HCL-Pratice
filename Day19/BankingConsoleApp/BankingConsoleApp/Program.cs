
using BankingConsoleApp.Data;
using BankingConsoleApp.Menus;
using BankingConsoleApp.Repositories;
using BankingConsoleApp.Services;
using Microsoft.EntityFrameworkCore;

const string connectionString =
    "Server=(localdb)\\MSSQLLocalDB;Database=BankingConsoleDb;Trusted_Connection=True;TrustServerCertificate=True;";

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var context = new AppDbContext(options);

// Repositories
var customerRepository = new CustomerRepository(context);
var accountRepository = new BankAccountRepository(context);
var branchRepository = new BranchRepository(context);
var employeeRepository = new EmployeeRepository(context);
var transactionRepository = new TransactionRepository(context);
var loanRepository = new LoanRepository(context);
var auditLogRepository = new AuditLogRepository(context);

// Services
var customerService = new CustomerService(customerRepository);
var accountService = new BankAccountService(accountRepository);
var branchService = new BranchService(branchRepository);
var employeeService = new EmployeeService(employeeRepository);
var transactionService = new TransactionService(transactionRepository);

var auditLogService = new AuditLogService(auditLogRepository);

var loanService = new LoanService(
    loanRepository,
    auditLogService);

// Individual menus
var customerMenu = new CustomerMenu(customerService);
var accountMenu = new BankAccountMenu(accountService, customerService);
var branchMenu = new BranchMenu(branchService);
var employeeMenu = new EmployeeMenu(employeeService);
var transactionMenu = new TransactionMenu(transactionService, accountService);
var loanMenu = new LoanMenu(loanService, customerService);
var auditLogMenu = new AuditLogMenu(auditLogService);

// Main menu
var mainMenu = new MainMenu(
    customerMenu,
    accountMenu,
    branchMenu,
    employeeMenu,
    transactionMenu,
    loanMenu,
    auditLogMenu);

await mainMenu.RunAsync();
