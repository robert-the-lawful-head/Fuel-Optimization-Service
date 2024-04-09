export const convertDMSToDEG = (dms:string): number => {   
    let dms_Array = dms.split(/[^\d]+/);

    let degrees = dms_Array[1];
    let minutes = dms_Array[2];
    let seconds = dms_Array[3];
    let direction = dms[0];

    let deg = Number((Number(degrees) + Number(minutes)/60 + Number(seconds)/3600).toFixed(6));

    if (direction == "S" || direction == "W") {
        deg = deg * -1;
    } // Don't do anything for N or E
    return deg;
}