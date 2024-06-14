import { Inject, OnInit } from '@angular/core';
import { Component, } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { JetNet } from '../../../models/jetnet-information';
import { CustomersDialogNewCustomerComponent } from '../../../pages/customers/customers-dialog-new-customer/customers-dialog-new-customer.component';
import { JetNetService } from '../../../services/jetnet.service';


@Component({
    selector: 'app-jetnet-information',
    templateUrl: './jetnet-information.component.html',
    styleUrls: ['./jetnet-information.component.scss'],
})
export class JetNetInformationComponent implements OnInit {
    public jetNetInformation: JetNet;
    public isLoading: boolean;
    public companyBusinessTypes: string[];
    public companyContacts: string[];

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        private newCustomerDialog: MatDialog,
        public jetNetDialogRef: MatDialogRef<JetNetInformationComponent>,
        private jetNetService: JetNetService
    ) { }

    ngOnInit() {
        this.isLoading = true;
        if (this.data && this.data.startsWith("N")) {
            this.jetNetService.getJetNetInformationByTailNumber(this.data).subscribe((response: JetNet) => {
                this.jetNetInformation = response;

                this.jetNetInformation.aircraftresult.companyrelationships.forEach((company) => {
                    company.add = true;
                });
                //var i = 1;
                //this.companyBusinessTypes = new Array<string>();
                //this.companyContacts= new Array<string>();
                //this.jetNetInformation.aircraftresult.companyrelationships.forEach((company) => {
                //    if (this.companyBusinessTypes.includes(company.companybusinesstype) == false) {
                //        this.companyBusinessTypes.push(company.companybusinesstype);
                //    }

                //    if (company.contactfirstname !=null && this.companyContacts.includes(company.contactfirstname + " " + company.contactlastname) == false) {
                //        this.companyContacts.push(company.contactfirstname + " " + company.contactlastname);
                //    };

                //    company.contactid = i;
                //    i++;
                //});
                this.isLoading = false;
            });
        }
        else
            return;
    }

    public onClickCloseJetNetModal() {
        this.jetNetDialogRef.close();
    }

    public addCustomer() {
        const dialogRef = this.newCustomerDialog.open(
            CustomersDialogNewCustomerComponent,
            {
                height: '500px',
                width: '1140px',
                data: this.jetNetInformation?.aircraftresult,
            },
        );

        dialogRef.afterClosed().subscribe((customerInfoByGroupId) => {
            if (!customerInfoByGroupId) {
                return;
            }
            else
                this.jetNetDialogRef.close();
        });
    }
}
