import { Injectable } from '@angular/core';
import { FormArray } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class PricingTemplateCalcService {

    constructor() { }
    public adjustCustomerMarginNextValues(index: number, customerMarginsFormArray: FormArray): void {
        for(let i: number = index; i < customerMarginsFormArray.length ; i++){
            let currentMinValue: number = parseFloat(customerMarginsFormArray.at(i).get('min').value);
            let currentMaxValue: number = parseFloat(customerMarginsFormArray.at(i).get('max').value);


            if(i == index){
                if(currentMinValue < currentMaxValue) break;
                customerMarginsFormArray.at(i).patchValue({
                    max: currentMinValue + 1,
                });
                continue;
            }

            currentMinValue = parseFloat(customerMarginsFormArray.at(i).get('min').value);
            let previousMaxValue: number = parseFloat(customerMarginsFormArray.at(i-1).get('max').value);

            if(currentMinValue > previousMaxValue) break;

            customerMarginsFormArray.at(i).patchValue({
                min: previousMaxValue + 1,
            });

            currentMinValue = customerMarginsFormArray.at(i).get('min').value;
            currentMaxValue = parseFloat(customerMarginsFormArray.at(i).get('max').value);

            if(currentMinValue <  currentMaxValue) break;

            customerMarginsFormArray.at(i).patchValue({
                max: currentMinValue + 1,
            });
        }
    }
    public adjustCustomerMarginPreviousValues(index: number, customerMarginsFormArray: FormArray): void {
        for(let i: number = index; i > 0 ; i--){
            let previousIndex: number = i - 1;
            let modifedMinValue: number = customerMarginsFormArray.at(i).get('min').value;
            let previousValueMax: number = customerMarginsFormArray.at(previousIndex).get('max').value;

            customerMarginsFormArray.at(previousIndex).patchValue({
                max: modifedMinValue - 1,
            });

            let previousValueMin: number = customerMarginsFormArray.at(previousIndex).get('min').value;
            previousValueMax = customerMarginsFormArray.at(previousIndex).get('max').value;

            if(previousValueMin >= previousValueMax){
                customerMarginsFormArray.at(previousIndex).patchValue({
                    min: previousValueMax - 1,
                });
            }
        }
    }
}
