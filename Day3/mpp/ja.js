function sss(){

    let weightinp = document.getElementById("e").value
    let heightinp = document.getElementById("ee").value
    let v=document.getElementById("es")
    let userData = {
    "weight": parseFloat(weightinp),
    "height": parseFloat(heightinp)/100,
    "cal":function(){
        return this.weight / (this.height *this.height);
    }
    
};
    let bmi = userData.cal();
    let stat=""

    if(bmi>25){
        stat= "overWeight"
        textcolor="red"        
    }
    else if(bmi<=18.4){
        stat = "under weight";
        textcolor="orange"

    }else{
        stat= "Healthy body";
        textcolor="green"


    }
    document.getElementById("y").innerHTML = `<h2>${v.value}</h2>`;
    document.getElementById("d").innerHTML = `<h3>Your BMI is ${bmi.toFixed(2)}-<span style="color:${textcolor};">${stat}</span></h3>`;

}
