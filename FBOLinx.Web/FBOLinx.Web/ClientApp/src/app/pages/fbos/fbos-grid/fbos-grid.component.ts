import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { FbosService } from '../../../services/fbos.service';
import { FboairportsService } from '../../../services/fboairports.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { FbosDialogNewFboComponent } from '../fbos-dialog-new-fbo/fbos-dialog-new-fbo.component';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/'
    },
    {
        title: 'FBOs',
        link: ''
    }
];

@Component({
    selector: 'app-fbos-grid',
    templateUrl: './fbos-grid.component.html',
    styleUrls: ['./fbos-grid.component.scss']
})
/** fbos-grid component*/
export class FbosGridComponent implements OnInit {

    //Input/Output Bindings
    @Output() recordDeleted = new EventEmitter<any>();
    @Output() newFboClicked = new EventEmitter<any>();
    @Output() editFboClicked = new EventEmitter<any>();
    @Input() fbosData: Array<any>;
    @Input() groupInfo: any;

    //Public Members
    public pageTitle: string = 'FBOs';
    public breadcrumb: any[] = BREADCRUMBS;
    public fbosDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ['icao', 'fbo', 'active', 'edit', 'delete'];
    public airportData: Array<any>;
    public resultsLength: number = 0;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    /** fbos-grid ctor */
    constructor(public newFboDialog: MatDialog,
        private fboService: FbosService,
        private fboAirportsService: FboairportsService,
        private sharedService: SharedService,
        public deleteFboDialog: MatDialog) {

        this.sharedService.emitChange(this.pageTitle);
    }

    ngOnInit() {
        if (!this.fbosData)
            return;
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
        this.fbosDataSource = new MatTableDataSource(this.fbosData);
        this.fbosDataSource.sort = this.sort;
        this.fbosDataSource.paginator = this.paginator;
        this.resultsLength = this.fbosData.length;
    }

    /** Public Methods */
    public deleteRecord(record) {
        const dialogRef = this.deleteFboDialog.open(DeleteConfirmationComponent, {
            data: { item: record, description: 'FBO' }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            const deleteIndex = this.fbosData.indexOf(record);
            this.fboService.remove(record).subscribe(
                result => { this.fbosData.splice(deleteIndex, 1); }
            );
            this.recordDeleted.emit(record);
        });
    }

    public editRecord(record) {
        const clonedRecord = Object.assign({}, record);
        this.editFboClicked.emit(clonedRecord);
    }

    public newRecord() {
        let airportData = this.airportData;
        const dialogRef = this.newFboDialog.open(FbosDialogNewFboComponent, {
            width: '450px',
            data: { oid: 0, initialSetupPhase: true }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('Dialog data: ', result);
            this.fboService.add(result).subscribe((data: any) => {
                if (!result.airport)
                    return;
                this.fboAirportsService.add({ oid: 0, fboId: data.oid, icao: result.airport.icao, iata: result.airport.iata })
                    .subscribe((fboAirportData:
                        any) => {
                        this.editRecord(data);
                    });
            });
        });
        this.newFboClicked.emit();
    }

    public applyFilter(filterValue: string) {
        this.fbosDataSource.filter = filterValue.trim().toLowerCase();
    }
}
