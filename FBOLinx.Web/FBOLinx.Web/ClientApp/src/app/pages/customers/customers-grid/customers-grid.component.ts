import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
    ElementRef,
} from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { MatDialog } from "@angular/material/dialog";
import * as _ from "lodash";

// Services
import { CustomeraircraftsService } from "../../../services/customeraircrafts.service";
import { CustomersService } from "../../../services/customers.service";
import { CustomerinfobygroupService } from "../../../services/customerinfobygroup.service";
import { SharedService } from "../../../layouts/shared-service";

// Components
import { CustomersDialogNewCustomerComponent } from "../customers-dialog-new-customer/customers-dialog-new-customer.component";
import { DeleteConfirmationComponent } from "../../../shared/components/delete-confirmation/delete-confirmation.component";

import * as XLSX from "xlsx";
import { CustomermarginsService } from "../../../services/customermargins.service";
import { CustomersviewedbyfboService } from "../../../services/customersviewedbyfbo.service";
import FlatfileImporter from "flatfile-csv-importer";
import { MatSelectChange } from "@angular/material/select";

@Component({
    selector: "app-customers-grid",
    templateUrl: "./customers-grid.component.html",
    styleUrls: ["./customers-grid.component.scss"],
})
export class CustomersGridComponent implements OnInit {
    // Input/Output Bindings
    @Output() editCustomerClicked = new EventEmitter<any>();
    @Output() customerDeleted = new EventEmitter<any>();

    @Input() customersData: any[];
    @Input() pricingTemplatesData: any[];
    @Input() aircraftsData: any[];

    // Public Members
    @ViewChild("customerTableContainer") table: ElementRef;
    public customersDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = [
        "selectAll",
        "needsAttention",
        "company",
        "customerCompanyTypeName",
        "pricingTemplateName",
        "allInPrice",
        "fleetSize",
        "delete",
    ];

    public customerFilterType = 0;

    public selectAll = false;
    public selectedRows: number;
    public globalMargin: any;
    public pageIndex = 0;
    public pageSize = 100;
    public tableSort = "needsAttention";
    public tableSortOrder = "asc";

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    LICENSE_KEY = "9eef62bd-4c20-452c-98fd-aa781f5ac111";

    results = "[]";

    private importer: FlatfileImporter;

    constructor(
        public newCustomerDialog: MatDialog,
        private customersService: CustomersService,
        private sharedService: SharedService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customersViewedByFboService: CustomersviewedbyfboService,
        public deleteCustomerDialog: MatDialog,
        public customeraircraftsService: CustomeraircraftsService,
        public customerMarginsService: CustomermarginsService
    ) {}

    ngOnInit() {
        this.refreshCustomerDataSource();

        FlatfileImporter.setVersion(2);
        this.initializeImporter();
        this.importer.setCustomer({
            userId: "1",
            name: "WebsiteImport",
        });

        if (localStorage.getItem("pageIndex")) {
            this.paginator.pageIndex = localStorage.getItem("pageIndex") as any;
        } else {
            this.paginator.pageIndex = 0;
        }

        if (sessionStorage.getItem("pageSizeValue")) {
            this.pageSize = sessionStorage.getItem("pageSizeValue") as any;
        } else {
            this.pageSize = 100;
        }

        if (sessionStorage.getItem("tableSortValue")) {
            this.tableSort = sessionStorage.getItem("tableSortValue") as any;
        }

        if (sessionStorage.getItem("tableSortValueDirection")) {
            this.tableSortOrder = sessionStorage.getItem(
                "tableSortValueDirection"
            ) as any;
        }
    }

    onPageChanged(event: any) {
        localStorage.setItem("pageIndex", event.pageIndex);
        sessionStorage.setItem(
            "pageSizeValue",
            this.paginator.pageSize.toString()
        );
        this.selectAll = false;
        _.forEach(this.customersData, (customer) => {
            customer.selectAll = false;
        });
    }

    // Public Methods
    public deleteCustomer(customer) {
        const dialogRef = this.deleteCustomerDialog.open(
            DeleteConfirmationComponent,
            {
                data: { item: customer, description: "customer" },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.customerInfoByGroupService
                .remove({ oid: result.item.customerInfoByGroupId })
                .subscribe(() => {
                    this.customerDeleted.emit();
                });
        });
    }

    public editCustomer(customer, $event) {
        if ($event.srcElement) {
            if (
                $event.srcElement.nodeName.toLowerCase() === "button" ||
                $event.srcElement.nodeName.toLowerCase() === "select" ||
                ($event.srcElement.nodeName.toLowerCase() === "input" &&
                    $event.srcElement.getAttribute("type") === "checkbox")
            ) {
                $event.stopPropagation();
                return;
            }
        }

        const clonedCustomer = Object.assign({}, customer);
        this.editCustomerClicked.emit(clonedCustomer);
    }

    public selectAction() {
        const pageCustomersData = this.customersDataSource.connect().value;
        _.forEach(pageCustomersData, (customer) => {
            customer.selectAll = this.selectAll ? true : false;
        });
        this.selectedRows = this.selectAll ? pageCustomersData.length : 0;
    }

    public selectUnique() {
        if (this.selectedRows === this.customersData.length) {
            this.selectAll = false;
            this.selectedRows = this.selectedRows - 1;
        }
    }

    public newCustomer() {
        const customerInfo = { oid: 0 };
        const dialogRef = this.newCustomerDialog.open(
            CustomersDialogNewCustomerComponent,
            {
                data: customerInfo,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.customersService.add(result).subscribe((data: any) => {
                result.customerId = data.oid;

                result.GroupId = this.sharedService.currentUser.groupId;

                this.customersViewedByFboService
                    .add({
                        fboId: this.sharedService.currentUser.fboId,
                        groupId: this.sharedService.currentUser.groupId,
                        customerId: result.customerId,
                    })
                    .subscribe(() => {});

                this.customerInfoByGroupService
                    .add(result)
                    .subscribe((customerInfoByGroupData: any) => {
                        result.customerInfoByGroupId =
                            customerInfoByGroupData.oid;
                        this.editCustomer(result, Event);
                    });
            });
        });
    }

    public applyFilter(filterValue: string) {
        this.customersDataSource.filter = filterValue.trim().toLowerCase();
    }

    public exportCustomersToExcel() {
        // Export the filtered results to an excel spreadsheet
        const filteredList = this.customersDataSource.filteredData.filter((item) => {
            return item.selectAll === true;
        });
        let exportData = [];
        if (filteredList.length > 0) {
            exportData = filteredList;
        } else {
            exportData = this.customersDataSource.filteredData;
        }
        exportData = _.map(exportData, (item) => {
            return {
                Company: item.company,
                Source:
                    item.customerCompanyTypeName === "FuelerLinx"
                        ? "FBOLinx Network"
                        : item.customerCompanyTypeName,
                "Assigned Price Tier": item.pricingTemplateName,
                Price: item.allInPrice,
            };
        });
        exportData = _.sortBy(exportData, [
            (item) => {
                return item.Company.toLowerCase();
            },
        ]);
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, "Customers");

        /* save to file */
        XLSX.writeFile(wb, "Customers.xlsx");
    }

    public alertHeader(value) {
        if (value) {
            sessionStorage.setItem("tableSortValue", value);
        }

        if (this.sort.direction) {
            sessionStorage.setItem(
                "tableSortValueDirection",
                this.sort.direction
            );
        }
    }

    public exportCustomerAircraftToExcel() {
        // Export the filtered results to an excel spreadsheet
        let exportData = _.map(this.aircraftsData, (item) => {
            let pricingTemplateName = item.pricingTemplateName;
            if (!pricingTemplateName) {
                const customer = _.find(this.customersData, (c) => {
                    return c.customerId === item.customerId;
                });
                if (customer) {
                    pricingTemplateName = customer.pricingTemplateName;
                }
            }
            const matchingPricingTemplate = _.find(
                this.pricingTemplatesData,
                (pricing) => {
                    return pricing.name === pricingTemplateName;
                }
            );
            return {
                Company: item.company,
                Tail: item.tailNumber,
                Make: item.make,
                Model: item.model,
                Size: item.aircraftSizeDescription,
                "Margin Template": pricingTemplateName,
                Pricing: matchingPricingTemplate
                    ? matchingPricingTemplate.intoPlanePrice
                    : "",
            };
        });
        exportData = _.sortBy(exportData, [
            (item) => {
                return item.Company.toLowerCase();
            },
            (item) => {
                return item.Tail.toLowerCase();
            },
        ]);
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, "Aircraft");

        /* save to file */
        XLSX.writeFile(wb, "Aircraft.xlsx");
    }

    public customerFilterTypeChanged(event) {
        this.refreshCustomerDataSource();
    }

    // Private Methods
    private refreshCustomerDataSource() {
        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.customersDataSource = new MatTableDataSource(
            this.customersData.filter((element: any) => {
                    if (this.customerFilterType === 0) {
                        return true;
                    }
                    return element.needsAttention;
                }
            )
        );
        this.sort.active = "allInPrice";
        this.customersDataSource.sort = this.sort;
        this.customersDataSource.paginator = this.paginator;
        this.customersDataSource.filterPredicate = (data: any, filter: string) => {
            if (filter === "needs attention") {
                return data.needsAttention === true;
            } else {
                let found = false;
                _.forOwn(data, (value) => {
                    if (value && value.toString().toLowerCase().includes(filter)) {
                        found = true;
                        return false;
                    }
                });
                return found;
            }
        };
    }

    public onMarginChange(newValue: any, customer: any) {
        const changedPricingTemplate = _.find(this.pricingTemplatesData, (p) => {
            return customer.pricingTemplateName === p.name;
        });

        customer.needsAttention = changedPricingTemplate.default;
        customer.allInPrice = changedPricingTemplate.intoPlanePrice;
        const vm = {
            id: customer.customerId,
            customerMarginName: newValue,
            fboid: this.sharedService.currentUser.fboId,
        };
        this.customerMarginsService
            .updatecustomermargin(vm)
            .subscribe(() => {
                // this.refreshCustomerDataSource();
            });
    }

    public bulkMarginTemplateUpdate(event: MatSelectChange) {
        const listCustomers = [];

        _.forEach(this.customersData, (customer) => {
            if (customer.selectAll === true) {
                customer.needsAttention = event.value.default;
                customer.pricingTemplateName = event.value.name;
                customer.allInPrice = event.value.intoPlanePrice;
                listCustomers.push({
                    id: customer.customerId,
                    customerMarginName: event.value.name,
                    fboid: this.sharedService.currentUser.fboId,
                });
            }
        });

        this.customerMarginsService
            .updatemultiplecustomermargin(listCustomers)
            .subscribe(() => {
                // this.refreshCustomerDataSource();
            });
    }

    public anySelected() {
        const filteredList = this.customersData.filter((item) => {
            return item.selectAll === true;
        });
        return filteredList.length > 0;
    }

    public async launchImporter() {
        if (!this.LICENSE_KEY) {
            return alert("Set LICENSE_KEY on Line 13 before continuing.");
        }
        try {
            const results = await this.importer.requestDataFromUser();
            this.importer.displayLoader();

            if (results) {
                results.data.forEach((result) => {
                    result.groupid = this.sharedService.currentUser.groupId;
                });

                this.customersService
                    .importcustomers(results.data)
                    .subscribe((data: any) => {
                        if (data) {
                            this.importer.displaySuccess(
                                "Data successfully imported!"
                            );
                            setTimeout(() => {
                                this.customerDeleted.emit();
                            }, 1500);
                        }
                    });
            }
        } catch (e) {}
    }

    public initializeImporter() {
        this.importer = new FlatfileImporter(this.LICENSE_KEY, {
            fields: [
                {
                    label: "Company Id",
                    alternates: ["Id", "CompanyId"],
                    key: "CompanyId",
                    description: "Company Id Value",
                },
                {
                    label: "CompanyName",
                    alternates: ["Company Name", "Name"],
                    key: "CompanyName",
                    description: "Company Name Value",
                    validators: [
                        {
                            validate: "required",
                            error: "this field is required",
                        },
                    ],
                },
                {
                    label: "Activate",
                    alternates: ["activate"],
                    key: "Activate",
                    description: "Activate Flag",
                },
                {
                    label: "Tail",
                    alternates: ["tail", "plane tail", "N-number", "Nnumber"],
                    key: "Tail",
                    description: "Tail",
                },
                {
                    label: "Aircraft Make",
                    alternates: [
                        "Make",
                        "make",
                        "aircraft make",
                        "aircraft",
                        "Manufacturer",
                        "Aircraft Manufacturer",
                    ],
                    key: "AircraftMake",
                    description: "Aircraft Make",
                },
                {
                    label: "Model",
                    alternates: [
                        "Aircraft Model",
                        "aircraft model",
                        "model",
                        "aircraft type",
                        "type",
                    ],
                    key: "AircraftModel",
                    description: "Aircraft Model",
                },
                {
                    label: "Size",
                    alternates: ["Aircraft Size", "Plane Size"],
                    key: "AircraftSize",
                    description: "Aircraft Size",
                },
                {
                    label: "First Name",
                    alternates: ["first name", "name"],
                    key: "FirstName",
                    description: "First Name",
                },
                {
                    label: "Last Name",
                    alternates: ["last name", "lname"],
                    key: "LastName",
                    description: "Last Name",
                },
                {
                    label: "Title",
                    alternates: ["title"],
                    key: "Title",
                    description: "Title",
                },
                {
                    label: "Email",
                    alternates: ["email address", "email"],
                    key: "Email",
                    description: "Email",
                },
                {
                    label: "Mobile",
                    alternates: ["mobile", "cell", "cell phone"],
                    key: "Mobile",
                    description: "Mobile",
                },
                {
                    label: "Phone",
                    alternates: ["phone"],
                    key: "Phone",
                    description: "Phone",
                },
            ],
            type: "Customers",
            allowInvalidSubmit: true,
            managed: true,
            allowCustom: true,
            disableManualInput: false,
        });
    }
}
