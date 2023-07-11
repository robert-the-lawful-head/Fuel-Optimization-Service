import { Component, OnInit, Input, ChangeDetectorRef, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SharedService } from 'src/app/layouts/shared-service';
import { FbosServicesAndFeesResponse, ServiceTypeResponse, ServicesAndFees, ServicesAndFeesResponse } from 'src/app/models/services-and-fees/services-and-fees';
import { ServiceTypeService } from 'src/app/services/serviceTypes.service';
import { ServicesAndFeesService } from 'src/app/services/servicesandfees.service';
import { DeleteConfirmationComponent } from 'src/app/shared/components/delete-confirmation/delete-confirmation.component';

interface ServicesAndFeesGridItem extends ServicesAndFeesResponse{
    isEditMode : boolean,
    isNewItem: boolean,
    editedValue: string
}
@Component({
  selector: 'app-services-and-fees',
  templateUrl: './services-and-fees.component.html',
  styleUrls: ['./services-and-fees.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ServicesAndFeesComponent implements OnInit {
    @Input() public fboId: number|null = null;
    servicesAndFeesGridDisplay: FbosServicesAndFeesResponse[] = [];

    constructor(
        private servicesAndFeesService: ServicesAndFeesService,
        private serviceTypeService: ServiceTypeService,
        private sharedService: SharedService,
        private deleteDialog: MatDialog,
        private snackBar: MatSnackBar
        ) { }

    async ngOnInit() {
        this.servicesAndFeesGridDisplay = await this.servicesAndFeesService.getFboServicesAndFees(this.sharedService.currentUser.fboId).toPromise();
    }

    createNewItem(serviceType: ServiceTypeResponse): void {

        let category = this.servicesAndFeesGridDisplay.find(elem=> elem.serviceType.name == serviceType.name);

        let newItem: ServicesAndFeesGridItem = {
            oid : 0,
            isNewItem : true,
            handlerId : category.servicesAndFees[0]?.handlerId,
            serviceOfferedId : category.servicesAndFees[0]?.serviceOfferedId,
            isCustom : true,
            serviceTypeId : serviceType.oid,
            service : '',
            isActive : true,
            isEditMode : true,
            editedValue : ''
        };


        category.servicesAndFees.push(newItem);
    }
    saveItem(serviceAndfee:  ServicesAndFeesGridItem): void {
       if(serviceAndfee.isNewItem)
            this.add(serviceAndfee);
        else
            this.update(serviceAndfee);
    }
    add(serviceAndfee:  ServicesAndFeesGridItem): void {
        serviceAndfee.service = serviceAndfee.editedValue;

        this.servicesAndFeesService.add(this.sharedService.currentUser.fboId, serviceAndfee)
        .subscribe(response => {
            console.log("🚀 ~ file: services-and-fees.component.ts:86 ~ ServicesAndFeesComponent ~ add ~ response:", response)

            serviceAndfee.oid = response.oid;
            serviceAndfee.isNewItem = false;
            this.toogleEditModel(serviceAndfee);
        }, error => {
            this.showErrorSnackBar( `There was an error saving ${serviceAndfee.service} ${serviceAndfee.service} please try again`);
            console.log(error);
            this.toogleEditModel(serviceAndfee);
        });
    }

    update(serviceAndfee :  ServicesAndFeesGridItem): void {
        let updatedItem: ServicesAndFees =  Object.assign({}, serviceAndfee);

        let previousNameBackup = serviceAndfee.service;
        updatedItem.service = serviceAndfee.editedValue;

        this.servicesAndFeesService.update(this.sharedService.currentUser.fboId,updatedItem).subscribe(response => {
            serviceAndfee.service = serviceAndfee.editedValue;
            this.toogleEditModel(serviceAndfee);
            this.showSuccessSnackBar( `Service ${serviceAndfee.service} was updated successfully`);
        }, error => {
            serviceAndfee.editedValue = previousNameBackup;
            serviceAndfee.service = previousNameBackup;
            this.showErrorSnackBar( `There was an error updating the service please try again`);
            console.log(error);
            this.toogleEditModel(serviceAndfee);
        });
    }
    updateActiveFlag(serviceAndfee :  ServicesAndFeesGridItem): void {
        let updatedItem: ServicesAndFees = {
            oid: serviceAndfee.oid,
            handlerId: serviceAndfee.handlerId,
            serviceOfferedId: serviceAndfee.serviceOfferedId,
            service: serviceAndfee.service,
            serviceTypeId: serviceAndfee.serviceTypeId,
            isActive: !serviceAndfee.isActive
        }
        this.servicesAndFeesService.update(this.sharedService.currentUser.fboId,updatedItem).subscribe(response => {
            serviceAndfee.oid = response.oid;
            this.showSuccessSnackBar( `Service ${serviceAndfee.service} was updated successfully`);
        }, error => {
            this.showErrorSnackBar( `There was an error updating the service please try again`);
            serviceAndfee.isActive = !serviceAndfee.isActive;
            console.log(error);

        });
    }
    toogleEditModel(item: ServicesAndFeesGridItem,itemList: ServicesAndFeesGridItem[] = []): void {
        if(item.isNewItem)
            itemList.pop();
        else
            item.isEditMode = !item.isEditMode;
    }
    deleteItem(serviceAndfee: ServicesAndFeesGridItem): void {
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
                let category = this.servicesAndFeesGridDisplay.find(elem=> elem.serviceType.oid == serviceAndfee.item.serviceTypeId);

                category.servicesAndFees = category.servicesAndFees.filter(item => item.oid != serviceAndfee.item.oid);

                this.toogleEditModel(serviceAndfee);
                this.showSuccessSnackBar("Service deleted successfully");
            }, error => {
                this.showErrorSnackBar( `There was an error deleting ${serviceAndfee.item.serviceType} ${serviceAndfee.item.service} please try again`);
                console.log(error);
            });
        });
    }
    createNewCategory(): void {
        let newItem: ServiceTypeResponse = {
            oid: 0,
            name: '',
            isCustom : true
        };
        // this.serviceTypeService.add(this.sharedService.currentUser.fboId, newItem)
        // .subscribe(response => {
        //     serviceAndfee.oid = response.oid;
        //     serviceAndfee.isNewItem = false;
        //     this.toogleEditModel(serviceAndfee);
        // }, error => {
        //     this.showErrorSnackBar( `There was an error saving ${serviceAndfee.serviceType} ${serviceAndfee.service} please try again`);
        //     console.log(error);
        //     this.toogleEditModel(serviceAndfee);
        // });
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
    private showSuccessSnackBar(message: string): void {
        this.snackBar.open(
            message,
            '',
            {
                duration: 2000,
                panelClass: ['blue-snackbar'],
            }
        );
    }
}
