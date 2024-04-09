import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from "@angular/material/paginator";
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource } from "@angular/material/table";
import {
    ColumnType,
    TableSettingsComponent,
} from 'src/app/shared/components/table-settings/table-settings.component';
import * as XLSX from 'xlsx';

export type csvFileOptions = {
    fileName: string;
    sheetName: string;
}

export abstract class GridBase {
    start: number = 0;
    limit: number = 10;
    pageSize: number = 30;
    selectedRowIndex: number = null;
    dataSource: any = new MatTableDataSource([]); //data source to be displayed on scroll

    constructor() {
        this.dataSource.sortingDataAccessor = (item, property) => {
            switch(property) {
              case 'eta': return new Date(item.eta);
              case 'etd': return new Date(item.etd);
              case 'createdDate': return new Date(item.createdDate);
              case 'fuelOn': return (item.fuelOn == "Arrival" ? new Date(item.eta) : new Date(item.etd));
              default:
                if (typeof item[property] === 'string') {
                    return item[property].toLocaleLowerCase();
                }
                return item[property];
            }
          }
    }

    onTableScroll(e): void {
        const tableViewHeight = e.target.offsetHeight; // viewport
        const tableScrollHeight = e.target.scrollHeight; // length of all table
        const scrollLocation = e.target.scrollTop; // how far user scrolled

        // If the user has scrolled within 200px of the bottom, add more data
        const buffer = 200;
        const limit = tableScrollHeight - tableViewHeight - buffer;
        if(this.pageSize > this.dataSource.data.length) return;
        if (scrollLocation > limit) {
            this.updateIndex();
            this.setPagination(this.pageSize);
        }
    }
    updateIndex() {
        this.start = this.pageSize;
        this.pageSize = this.limit + this.start;
    }
    setVirtualScrollVariables(paginator: MatPaginator, sort: MatSort, data: any[]){
        this.dataSource.paginator = paginator;
        this.dataSource.sort = sort;
        this.dataSource.data = data;
        this.setPagination(this.pageSize);
    }
    setPagination(paginationSize: number){
        this.dataSource.paginator.pageSize = paginationSize;
        this.dataSource.paginator.page.emit({
                length: 1,
              pageIndex: 0,
              pageSize: paginationSize,
              })
    }
    onPageChanged(event: any) {
        // localStorage.setItem('pageIndex', event.pageIndex);
        // sessionStorage.setItem(
        //     'pageSizeValue',
        //     this.dataSource.paginator.pageSize.toString()
        // );
        // this.selectAll = false;
        // forEach(this.dataSource, (data) => {
        //     data.selectAll = false;
        // });
    }
    exportCsvFile(columns: ColumnType[],fileName: string, sheetName: string, computePropertyFnc: any,exportSelectedData: boolean = false) {

        let cols = columns.filter(col => (col.id == 'selectAll')? false : !col.hidden);

        let dataToExport = (exportSelectedData) ?
        this.dataSource.filteredData.filter(x => x.selectAll) :
        this.dataSource.filteredData;

        let exportData = dataToExport.map((item) => {
            let row = {};
            cols.forEach( col => {
                let calculatedText =  computePropertyFnc(item, col.id);
                row[col.name] = calculatedText ? calculatedText : item[col.id];
            });
            return row;
        });
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(
            wb,
            ws,
            sheetName
        );

        /* save to file */
        XLSX.writeFile(wb, fileName +'.xlsx');
    }
    public getClientSavedColumns(tableLocalStorageKey: string, initialColumns: ColumnType[]): ColumnType[]{
        let localStorageColumns: string = localStorage.getItem(tableLocalStorageKey);

        if (!localStorageColumns) return initialColumns;

        let storedCols: ColumnType[] = JSON.parse(localStorageColumns);

        initialColumns.sort((a, b) => {
            var indexOfA = storedCols.findIndex(x => x.id == a.id);
            var indexOfB = storedCols.findIndex(x => x.id == b.id);
            return indexOfA - indexOfB;
        });

        return initialColumns.map(initialCol => {
            const storedColumn = storedCols.find(storedCol => initialCol.id === storedCol.id);
            if(storedColumn){
                initialCol.hidden = storedColumn.hidden;
            }
            return initialCol;
        });
    }

    public openSettingsDialog(tableSettingsDialog: MatDialog, columns: ColumnType[], onCompletion) {
        const dialogRef = tableSettingsDialog.open(
            TableSettingsComponent,
            {
                data: columns,
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            columns = [...result];

            if (onCompletion)
                onCompletion(result);
        });
    }

    public saveSettings(key: string, columns: ColumnType[]) {
        localStorage.setItem(
            key,
            JSON.stringify(columns)
        );
    }

    public refreshSort(sort: MatSort, columns: ColumnType[]) {
        if (!sort)
            return;
        const sortedColumn = columns.find(
            (column) => !column.hidden && column.sort
        );
        sort.sort({
            disableClear: false,
            id: null,
            start: sortedColumn?.sort || 'asc',
        });
        sort.sort({
            disableClear: false,
            id: sortedColumn?.id,
            start: sortedColumn?.sort || 'asc',
        });
        (
            sort.sortables.get(sortedColumn?.id) as MatSortHeader
        )?._setAnimationTransitionState({ toState: 'active' });
    }
    getEndOfDayTime(date: Date, utcDate: boolean = false): Date {
        return (utcDate) ?
        new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), 23, 59, 59)):
        new Date(date.getFullYear(), date.getMonth(), date.getDate(), 23, 59, 59);
    }
    getStartOfDayTime(date: Date, utcDate: boolean = false): Date {
        return (utcDate) ?
        new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0)):
        new Date(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0);
    }
}
