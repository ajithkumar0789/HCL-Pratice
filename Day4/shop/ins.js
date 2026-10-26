function sss(){
    let e1 = document.getElementById("e1").value || 0;
    let e2 = document.getElementById("e2").value || 0;
    let e3 = document.getElementById("e3").value || 0;
    let e4 = document.getElementById("e4").value || 0;
    let e5 = document.getElementById("e5").value || 0;
    let e6 = document.getElementById("e6").value || 0;
    let e7 = document.getElementById("e7").value || 0;
    let e8 = document.getElementById("e8").value || 0;
    let e9 = document.getElementById("e9").value || 0;

    let userData = {
        "rice": parseInt(e1),
        "wheat": parseInt(e2),
        "milk": parseInt(e3),
        "paste": parseInt(e4),
        "brush": parseInt(e5),
        "js": parseInt(e6),
        "co": parseInt(e7),
        "ch": parseInt(e8),
        "ck": parseInt(e9),
        
        "cal": function() {
            return (this.rice * 10 + this.wheat * 20 + this.milk * 30 + this.paste * 40 + this.brush * 50 + this.js * 60 + this.co * 70 + this.ch * 80 + this.ck * 90);
},
        "itemsCount": function() {
            return (this.rice + this.wheat + this.milk + this.paste + this.brush + this.js + this.co + this.ch + this.ck);
}
    };

    let tot = userData.cal();
    let totalQty = userData.itemsCount();
    document.getElementById("result").innerHTML = `Total Items: ${totalQty} | Total Bill: ${tot}`;
};