export const convertDMSToDEG = (dms:string): number => {   
    var dms_Array = dms.split(/[^\d]+/);

    var degrees = dms_Array[1];
    var minutes = dms_Array[2];
    var seconds = dms_Array[3];
    var direction = dms[0];

    var deg = Number((Number(degrees) + Number(minutes)/60 + Number(seconds)/3600).toFixed(6));

    if (direction == "S" || direction == "W") {
        deg = deg * -1;
    } // Don't do anything for N or E
    return deg;
}