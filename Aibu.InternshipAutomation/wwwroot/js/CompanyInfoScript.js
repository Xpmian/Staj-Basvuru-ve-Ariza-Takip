function validate(){
    let compPhone = document.getElementById("comp_phone").value;
    let fax = document.getElementById("fax").value;
    let phoneNum = document.getElementById("phone-num").value;
    let employees = document.getElementById("employees").value;

    let rephone =  /^\d{10}$/;
    let refax = /^\d{10}$/;
    let reemployees = /^\d+(?:-\d+)?$/;

    if(!rephone.test(compPhone)){
    
        return false;
    }
    if(!refax.test(fax)){
      
        return false;
    }
    if(!rephone.test(phoneNum)){
        
        return false;
    }
    if(!reemployees.test(employees)){
    
        return false;
    }
    return true;
}
