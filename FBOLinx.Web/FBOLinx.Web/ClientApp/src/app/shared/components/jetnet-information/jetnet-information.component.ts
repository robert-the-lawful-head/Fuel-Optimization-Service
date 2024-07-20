import { Inject, OnInit } from '@angular/core';
import { Component, } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Company, JetNet } from '../../../models/jetnet-information';
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
    public isExpanded: boolean = false;
    private expandedPanels: Array<string> = new Array();

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

                this.jetNetInformation.aircraftresult.companies.forEach((company) => {
                    company.companyrelationships.forEach((contact) => {
                        if (contact.contactemail)
                            contact.add = true;
                    });
                });
                this.isLoading = false;
            });
        }
        else
            return;
    }

    onChange(company: Company, state: string) {
        if (state == "opened") {
            if (this.expandedPanels == undefined)
                this.expandedPanels = new Array();

            if (this.expandedPanels.indexOf(company.company) < 0)
                this.expandedPanels.push(company.company);
        }
        else {
            this.expandedPanels = this.expandedPanels.filter((expandedCompany) => { return expandedCompany != company.company });
        }

        if (company.companyDetailOpenState == undefined)
            company.companyDetailOpenState = false;

        company.companyDetailOpenState = !company.companyDetailOpenState;
        if (company.companyDetailOpenState)
            this.isExpanded = true;
        else if (this.expandedPanels.length == 0)
            this.isExpanded = false;
    }

    public onClickCloseJetNetModal() {
        this.jetNetDialogRef.close();
    }

    public addCustomer() {
        const dialogRef = this.newCustomerDialog.open(
            CustomersDialogNewCustomerComponent,
            {
                height: '600px',
                width: '1140px',
                data: this.jetNetInformation?.aircraftresult,
            },
        );

        dialogRef.afterClosed().subscribe((customerInfoByGroupId) => {
            if (!customerInfoByGroupId) {
                return;
            }
            else
                this.jetNetDialogRef.close(customerInfoByGroupId);
        });
    }
}
