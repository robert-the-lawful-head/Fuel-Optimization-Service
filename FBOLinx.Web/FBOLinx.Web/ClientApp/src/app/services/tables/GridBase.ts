import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { MatLegacyPaginator as MatPaginator } from '@angular/material/legacy-paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatLegacyTableDataSource as MatTableDataSource } from '@angular/material/legacy-table';
import { PresetDateFilterEnum, SelectedDateFilter } from 'src/app/shared/components/preset-date-filter/preset-date-filter.component';
import {
    ColumnType,
    TableSettingsComponent,
} from 'src/app/shared/components/table-settings/table-settings.component';
import * as XLSX from 'xlsx-js-style';
import * as moment from 'moment';

export type csvFileOptions = {
    fileName: string;
    sheetName: string;
};

export abstract class GridBase {
    start: number = 0;
    limit: number = 10;
    pageSize: number = 30;
    selectedRowIndex: number = null;
    dataSource: any = new MatTableDataSource([]); //data source to be displayed on scroll
    //might be good to set as part of grid base but this involves changing all grid, and remove from function parameters
    //think about removing the setVirtualScrollVariables function and set directly in parent 
    //columns: ColumnType = [];
    //tableLocalStorageKey = [];

    //for reports grid
    private _icaoFilter: string;
    filterStartDate: Date = new Date(
        moment().add(-1, 'M').format('MM/DD/YYYY')
    );
    filterEndDate: Date = new Date(moment().format('MM/DD/YYYY'));

    selectedDateFilter: SelectedDateFilter;
    hiddenColumns: string[] = [
        'directOrders',
        'conversionRate',
        'conversionRateTotal',
        'totalOrders',
        'customerBusiness',
        'airportVisits',
    ];

    constructor() {
        this.selectedDateFilter = {
            offsetDate: this.filterStartDate,
            limitDate: this.filterEndDate,
            selectedFilter : PresetDateFilterEnum.oneMonth
        };
        this.dataSource.sortingDataAccessor = (item, property) => {
            switch (property) {
                case 'eta':
                    return new Date(item.eta);
                case 'etd':
                    return new Date(item.etd);
                case 'createdDate':
                    return new Date(item.createdDate);
                case 'fuelOn':
                    return item.fuelOn == 'Arrival'
                        ? new Date(item.eta)
                        : new Date(item.etd);
                default:
                    if (typeof item[property] === 'string') {
                        return item[property].toLocaleLowerCase();
                    }
                    return item[property];
            }
        };
    }
    get icaoFilter(): string {
        return this._icaoFilter;
    }
    set icaoFilter(value: string) {
        this._icaoFilter = value;
    }
    onTableScroll(e): void {
        const tableViewHeight = e.target.offsetHeight; // viewport
        const tableScrollHeight = e.target.scrollHeight; // length of all table
        const scrollLocation = e.target.scrollTop; // how far user scrolled

        // If the user has scrolled within 200px of the bottom, add more data
        const buffer = 200;
        const limit = tableScrollHeight - tableViewHeight - buffer;
        if (this.pageSize > this.dataSource.data.length) return;
        if (scrollLocation > limit) {
            this.updateIndex();
            this.setPagination(this.pageSize);
        }
    }
    updateIndex() {
        this.start = this.pageSize;
        this.pageSize = this.limit + this.start;
    }
    setVirtualScrollVariables(
        paginator: MatPaginator,
        sort: MatSort,
        data: any[]
    ) {
        this.dataSource.paginator = paginator;
        this.dataSource.sort = sort;
        this.dataSource.data = data;
        this.setPagination(this.pageSize);
    }
    setPagination(paginationSize: number) {
        this.dataSource.paginator.pageSize = paginationSize;
        this.dataSource.paginator.page.emit({
            length: 1,
            pageIndex: 0,
            pageSize: paginationSize,
        });
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
    exportCsvFile(
        columns: ColumnType[],
        fileName: string,
        sheetName: string,
        computePropertyFnc: any,
        exportSelectedData: boolean = false
    ) {
        let cols = columns.filter((col) =>
            col.id == 'selectAll' ? false : !col.hidden
        );

        let dataToExport = exportSelectedData
            ? this.dataSource.filteredData.filter((x) => x.selectAll)
            : this.dataSource.filteredData;

        let exportData = dataToExport.map((item) => {
            let row = {};
            cols.forEach((col) => {
                let calculatedText = !computePropertyFnc
                    ? ''
                    : computePropertyFnc(item, col.id);
                row[col.name] = calculatedText ? calculatedText : item[col.id];
            });
            return row;
        });
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet

        // Make headers bold
        const headerStyle: XLSX.CellStyle = {
            font: { bold: true },
            alignment: {
                horizontal: 'center',
                wrapText: true,
                vertical: 'center',
            },
        };

        // Get the range of cells in the first row (headers)
        const range = XLSX.utils.decode_range(ws['!ref']);
        for (let colIndex = range.s.c; colIndex <= range.e.c; colIndex++) {
            const cellAddress = { c: colIndex, r: range.s.r }; // First row index is range.s.r

            if (ws[XLSX.utils.encode_cell(cellAddress)]) {
                ws[XLSX.utils.encode_cell(cellAddress)].s = headerStyle;
            }
        }

        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, sheetName);

        /* save to file */
        XLSX.writeFile(wb, fileName + '.xlsx');
    }
    public getClientSavedColumns(
        tableLocalStorageKey: string,
        initialColumns: ColumnType[]
    ): ColumnType[] {
        let localStorageColumns: string =
            localStorage.getItem(tableLocalStorageKey);

        if (!localStorageColumns){
            initialColumns.sort((a, b) => {
                var indexOfA = initialColumns.findIndex((x) => x.index == a.index);
                var indexOfB = initialColumns.findIndex((x) => x.index == b.index);
                return indexOfA - indexOfB;
            });
            return initialColumns;
        }

        let storedCols: ColumnType[] = JSON.parse(localStorageColumns);

        initialColumns.sort((a, b) => {
            var indexOfA = storedCols.findIndex((x) => x.id == a.id);
            var indexOfB = storedCols.findIndex((x) => x.id == b.id);
            return indexOfA - indexOfB;
        });

        return initialColumns.map((initialCol) => {
            const storedColumn = storedCols.find(
                (storedCol) => initialCol.id === storedCol.id
            );
            if (storedColumn) {
                initialCol.hidden = storedColumn.hidden;
            }
            return initialCol;
        });
    }

    public openSettingsDialog(
        tableSettingsDialog: MatDialog,
        columns: ColumnType[],
        onCompletion
    ) {
        const dialogRef = tableSettingsDialog.open(TableSettingsComponent, {
            data: columns,
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            columns = [...result];

            if (onCompletion) onCompletion(result);
        });
    }

    public saveSettings(key: string, columns: ColumnType[]) {
        localStorage.setItem(key, JSON.stringify(columns));
    }

    public refreshSort(sort: MatSort, columns: ColumnType[]) {
        if (!sort) return;
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
    public getFilteredTableColumns(columns) {
        return columns
            .filter((column) => !column.hidden)
            .map((column) => column.id);
    }
    // todo: methods used on grids, need to be move into an utils service since not all grids use it
    public getEndOfDayTime(date: Date, utcDate: boolean = false): Date {
        return utcDate
            ? new Date(
                  Date.UTC(
                      date.getFullYear(),
                      date.getMonth(),
                      date.getDate(),
                      23,
                      59,
                      59
                  )
              )
            : new Date(
                  date.getFullYear(),
                  date.getMonth(),
                  date.getDate(),
                  23,
                  59,
                  59
              );
    }
    public getStartOfDayTime(date: Date, utcDate: boolean = false): Date {
        return utcDate
            ? new Date(
                  Date.UTC(
                      date.getFullYear(),
                      date.getMonth(),
                      date.getDate(),
                      0,
                      0,
                      0
                  )
              )
            : new Date(
                  date.getFullYear(),
                  date.getMonth(),
                  date.getDate(),
                  0,
                  0,
                  0
              );
    }
}
