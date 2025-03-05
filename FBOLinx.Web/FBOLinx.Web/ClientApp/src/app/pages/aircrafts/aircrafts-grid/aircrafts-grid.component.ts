import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { MatLegacyPaginator as MatPaginator } from '@angular/material/legacy-paginator';
import { MatLegacySelectChange as MatSelectChange } from '@angular/material/legacy-select';
import { MatSort } from '@angular/material/sort';
import { MatLegacyTableDataSource as MatTableDataSource } from '@angular/material/legacy-table';
import { ActivatedRoute } from '@angular/router';
import { csvFileOptions, GridBase } from 'src/app/services/tables/GridBase';
import { ColumnType } from 'src/app/shared/components/table-settings/table-settings.component';
import { NullOrEmptyToDefault } from 'src/app/shared/pipes/null/NullOrEmptyToDefault.pipe';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { CustomerAircraftsEditComponent } from '../../customer-aircrafts/customer-aircrafts-edit/customer-aircrafts-edit.component';
import { CallbackComponent } from 'src/app/shared/components/favorite-icon/favorite-icon.component';
import { defaultStringsEnum } from 'src/app/enums/strings.enums';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-aircrafts-grid',
    styleUrls: ['./aircrafts-grid.component.scss'],
    templateUrl: './aircrafts-grid.component.html',
    providers: [NullOrEmptyToDefault]
})
export class AircraftsGridComponent extends GridBase implements OnInit {
    // Input/Output Bindings
    @Output() editAircraftClicked = new EventEmitter<any>();
    @Output() refreshAircrafts = new EventEmitter<any>();
    @Input() aircraftsData: Array<any>;
    @Input() pricingTemplatesData: Array<any>;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    // Public Members
    public aircraftsDataSource: MatTableDataSource<any> = null;
    public columns: ColumnType[] = [
        { id: 'tailNumber', name: 'Tail Number'},
        { id: 'aircraftType', name: 'Aircraft Type'},
        { id: 'aircraftSize',name: 'Aircraft Size'},
        { id: 'company', name: 'Company'},
        { id: 'aircraftPricingTemplate', name: 'Aircraft Pricing Template'},
    ];
    displayedColumns = this.columns.map(function(item) {
        return item.id;
      });
    public resultsLength = 0;
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public isLoadingAircraftTypes = false;
    customersCsvOptions: csvFileOptions = { fileName: 'Aircraft', sheetName: 'Aircraft' };

    sortChangeSubscription: Subscription;

    constructor(
        public newCustomerAircraftDialog: MatDialog,
        public editCustomerAircraftDialog: MatDialog,
        private aircraftsService: AircraftsService,
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService ,
        private route : ActivatedRoute,
        private nullOrEmptyToDefault: NullOrEmptyToDefault
    ) {
        super();
        this.isLoadingAircraftTypes = true;
        this.aircraftsService.getAll().subscribe((data: any) => {
            this.aircraftTypes = data;
            this.isLoadingAircraftTypes = false;
        }, (error: any) => {
            //Error from service - set the loading flag to false so it tries again momentarily
            this.isLoadingAircraftTypes = false;
        });
    }

    ngOnInit() {
        if (!this.aircraftsData) {
            return;
        }

        this.sortChangeSubscription = this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.aircraftsDataSource = new MatTableDataSource(this.aircraftsData);
        this.aircraftsDataSource.sort = this.sort;
        this.aircraftsDataSource.paginator = this.paginator;

        this.resultsLength = this.aircraftsData.length;
        this.aircraftsDataSource.sortingDataAccessor = (item, property) => {
            switch (property) {
                case 'aircraftType':
                    return item.make + ' ' + item.model;
                case 'aircraftSize':
                    switch (item.size) {
                        case 8: // Single Engine Piston
                            return 1;
                        case 11: // Light Twin
                            return 2;
                        case 12: // Heavy Twin
                            return 3;
                        case 4: // Light Helicopter
                            return 4;
                        case 9: // Medium Helicopter
                            return 5;
                        case 10: // Heavy Helicopter
                            return 6;
                        case 13: // Light Turboprop
                            return 7;
                        case 6: // Single Turboprop
                            return 8;
                        case 14: // Medium Turboprop
                            return 9;
                        case 15: // Heavy Turboprop
                            return 10;
                        case 7: // Very Light Jet
                            return 11;
                        case 1: // Light Jet
                            return 12;
                        case 2: // Medium Jet
                            return 13;
                        case 3: // Heavy Jet
                            return 14;
                        case 16: // Super Heavy Jet
                            return 15;
                        case 5: // Wide Body
                            return 16;
                        default:
                            return 17;
                    }
                case 'aircraftPricingTemplate':
                    return item.pricingTemplateName;
                default:
                    return item[property];
            }
        };
        this.setVirtualScrollVariables(this.paginator, this.sort, this.aircraftsDataSource.data);
    }
    ngOnChanges(changes: SimpleChanges): void {
        if(changes.aircraftsData) {
            this.dataSource.data = changes.aircraftsData.currentValue;
        }
    }
    ngOnDestroy(){
        this.sortChangeSubscription?.unsubscribe();
    }
    public editCustomerAircraft(customerAircraft) {
        if (customerAircraft) {
            const dialogRef = this.editCustomerAircraftDialog.open(
                CustomerAircraftsEditComponent,
                {
                    data: {
                        disableDelete:
                            customerAircraft.isFuelerlinxNetwork &&
                            customerAircraft.addedFrom === 1,
                        oid: customerAircraft.oid,
                    },
                    width: '450px',
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }
                const id = this.route.snapshot.paramMap.get('id');
                if (result.toDelete) {
                    this.customerAircraftsService
                        .remove(result , this.sharedService.currentUser.oid)
                        .subscribe(() => {
                            this.refreshAircrafts.emit();
                        });
                }

                let aircraftSizes = [];
                this.aircraftsService
                    .getAircraftSizes()
                    .subscribe((data: any) => {
                        if (data) {
                            aircraftSizes = data;
                        }
                    });

                this.customerAircraftsService
                    .get({ oid: result.oid })
                    .subscribe((data: any) => {
                        if (data) {
                            const selectedAircraft = this.aircraftsData.find(
                                (x) => x.oid === result.oid
                            );

                            if (selectedAircraft) {
                                selectedAircraft.tailNumber = data.tailNumber;
                                selectedAircraft.make = data.make;
                                selectedAircraft.model = data.model;
                                selectedAircraft.size = data.size;

                                if (data.size) {
                                    const sizeText = aircraftSizes.find(
                                        (x) => x.value === data.size
                                    );

                                    if (sizeText) {
                                        selectedAircraft.aircraftSizeDescription =
                                            sizeText.description;
                                    }
                                }
                            }
                        }
                    });

                this.editAircraftClicked.emit(result);
            });
        }
    }

    public applyFilter(filterValue: string) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    public onMarginChange(event: MatSelectChange, customerAircraft: any) {
        const {
            aircraftId,
            customerId,
            groupId,
            make,
            model,
            oid,
            pricingTemplateId,
            size,
            tailNumber,
        } = customerAircraft;
        this.customerAircraftsService
            .updateTemplate(this.sharedService.currentUser.fboId, {
                aircraftId,
                customerId,
                groupId,
                make,
                model,
                oid,
                oldPricingTemplateId: pricingTemplateId,
                pricingTemplateId: event.value,
                size,
                tailNumber,
            })
            .subscribe((data: any) => {
                customerAircraft.pricingTemplateId = event.value;
            });
    }
    exportCsv() {
        let computePropertyFnc = (item: any[], id: string): any => {
            if(id == "aircraftType")
                return this.getAircrafttypeDisplayString(item);
            else if(id == "aircraftSize")
                return item['aircraftSizeDescription'];
            else
                return null;
        }
        this.exportCsvFile(this.columns,this.customersCsvOptions.fileName,this.customersCsvOptions.sheetName,computePropertyFnc);
    }
    getAircrafttypeDisplayString(aircraft: any): string {
        return this.nullOrEmptyToDefault.transform(aircraft.make,defaultStringsEnum.empty) +' '+ this.nullOrEmptyToDefault.transform(aircraft.model,defaultStringsEnum.empty);
    }
    setIsFavoriteProperty(aircraft: any): any {
        aircraft.isFavorite = aircraft.favoriteAircraft != null;
        return aircraft;
    }
    get getCallBackComponent(): CallbackComponent{
        return CallbackComponent.aircraft;
    }
}
