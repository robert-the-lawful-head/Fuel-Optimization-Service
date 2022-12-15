import { MatTableDataSource } from "@angular/material/table";

export abstract class VirtualScrollBase {
    start: number = 0;
    limit: number = 20;
    end: number = this.limit + this.start;
    selectedRowIndex: number = null;
    data: any = null; //all data on table
    dataSource: MatTableDataSource<any> = new MatTableDataSource<any>(); //data source to be displayed on scroll

    constructor() {}

    onTableScroll(e): void {
        const tableViewHeight = e.target.offsetHeight; // viewport
        const tableScrollHeight = e.target.scrollHeight; // length of all table
        const scrollLocation = e.target.scrollTop; // how far user scrolled

        // If the user has scrolled within 200px of the bottom, add more data
        const buffer = 200;
        const limit = tableScrollHeight - tableViewHeight - buffer;
        if (scrollLocation > limit) {
            let data = this.getTableData(this.start, this.end);
            this.dataSource.data = this.dataSource.data.concat(data);
            this.updateIndex();
        }
    }

    getTableData(start, end) {
        return this.data.filter(
            (value, index) => index >= start && index < end
        );
    }

    updateIndex() {
        this.start = this.end;
        this.end = this.limit + this.start;
    }

    selectedRow(row) {
        console.log('selectedRow', row);
    }
}
