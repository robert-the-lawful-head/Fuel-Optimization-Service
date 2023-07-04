import { Component, OnInit, Input, ChangeDetectorRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SharedService } from 'src/app/layouts/shared-service';
import { ServicesAndFees, ServicesAndFeesService } from 'src/app/services/servicesandfees.service';
import { DeleteConfirmationComponent } from 'src/app/shared/components/delete-confirmation/delete-confirmation.component';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/services-and-fees',
        title: 'FBO Services and Fees',
    },
];

interface ServicesAndFeesGridItem extends ServicesAndFees{
    isEditMode : boolean,
    isNewItem: boolean,
    editedValue: string
}
@Component({
  selector: 'app-services-and-fees',
  templateUrl: './services-and-fees.component.html',
  styleUrls: ['./services-and-fees.component.scss']
})
export class ServicesAndFeesComponent implements OnInit {
    @Input() public fboId: number|null = null;
    servicesAndFeesGridDisplay: { [key: string]: ServicesAndFeesGridItem[] } = {};
    breadcrumb = BREADCRUMBS;

    constructor(
        private servicesAndFeesService: ServicesAndFeesService,
        private sharedService: SharedService,
        private deleteDialog: MatDialog,
        private snackBar: MatSnackBar
        ) { }

    async ngOnInit() {
         var servicesAndFees = await this.servicesAndFeesService.getFboServicesAndFees(this.sharedService.currentUser.fboId).toPromise();
         servicesAndFees.forEach((serviceAndFee) => {
            this.addItemToServiceType(serviceAndFee);
         });
    }
    private addItemToServiceType(serviceAndFee: ServicesAndFees,isEditMode: boolean = false, isNewItem: boolean = false): void {
        let servicesAndFeesGrid: ServicesAndFeesGridItem = {
            ... serviceAndFee,
             isEditMode: isEditMode,
             isNewItem : isNewItem,
             editedValue: serviceAndFee.service
            };
        if(serviceAndFee.serviceType in this.servicesAndFeesGridDisplay)
            this.servicesAndFeesGridDisplay[serviceAndFee.serviceType].push(servicesAndFeesGrid);
        else
            this.servicesAndFeesGridDisplay[serviceAndFee.serviceType] = [servicesAndFeesGrid];
    }
    createNewItem(serviceAndFee: ServicesAndFeesGridItem): void {
        let newItem: ServicesAndFeesGridItem =  Object.assign({}, serviceAndFee);
        newItem.service = "";
        newItem.isNewItem = true;
        newItem.oid = 0;
        this.addItemToServiceType(newItem,true,true);
    }
    saveItem(serviceAndfee:  ServicesAndFeesGridItem): void {
        serviceAndfee.service = serviceAndfee.editedValue;
        let newItem: ServicesAndFees =  Object.assign({}, serviceAndfee);
        this.servicesAndFeesService.add(this.sharedService.currentUser.fboId, newItem)
        .subscribe(response => {
            serviceAndfee.isNewItem = false;
            this.toogleEditModel(serviceAndfee);
        }, error => {
            this.showErrorSnackBar( `There was an error saving ${serviceAndfee.serviceType} ${serviceAndfee.service} please try again`);
            console.log(error);
            this.toogleEditModel(serviceAndfee);
        });
    }
    updateItem(serviceAndfee :  ServicesAndFeesGridItem): void {
        let updatedItem: ServicesAndFees =  Object.assign({}, serviceAndfee);

        let previousNameBackup = serviceAndfee.service;
        updatedItem.service = serviceAndfee.editedValue;
        updatedItem.oid = 29999999999999;

        this.servicesAndFeesService.update(this.sharedService.currentUser.fboId,updatedItem).subscribe(response => {
            this.toogleEditModel(serviceAndfee);
        }, error => {
            serviceAndfee.editedValue = previousNameBackup;
            serviceAndfee.service = previousNameBackup;
            this.showErrorSnackBar( `There was an error updating the service please try again`);
            console.log(error);
        });
    }
    toogleEditModel(item: ServicesAndFeesGridItem): void {
        if(item.isNewItem)
            this.servicesAndFeesGridDisplay[item.serviceType].pop();
        else
            item.isEditMode = !item.isEditMode;
    }
    public deleteItem(serviceAndfee: ServicesAndFeesGridItem): void {
        const dialogRef = this.deleteDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'Service', item: serviceAndfee },
            }
        );

        dialogRef.afterClosed().subscribe((serviceAndfee) => {
            if (!serviceAndfee) {
                return;
            }
            this.servicesAndFeesService.remove(serviceAndfee.item.oid).subscribe(response => {
                this.servicesAndFeesGridDisplay[serviceAndfee.item.serviceType] = this.servicesAndFeesGridDisplay[serviceAndfee.item.serviceType].filter(item => item.oid !== serviceAndfee.item.oid);

                this.toogleEditModel(serviceAndfee);
            }, error => {
                this.showErrorSnackBar( `There was an error deleting ${serviceAndfee.item.serviceType} ${serviceAndfee.item.service} please try again`);
                console.log(error);
            });
        });
    }
    private showErrorSnackBar(message: string): void {
        this.snackBar.open(
            message,
            '',
            {
                duration: 2000,
                panelClass: ['error-snackbar'],
            }
        );
    }
}
