import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-favorite-icon',
  templateUrl: './favorite-icon.component.html',
  styleUrls: ['./favorite-icon.component.scss']
})
export class FavoriteIconComponent implements OnInit {
    @Input() favoriteData: any;
    @Input() isSaving: boolean = false;
    @Output() favoriteClick = new EventEmitter<any>();

    constructor() { }

    ngOnInit() {
    }
    toogleFavorite(): void{
        if(this.isSaving) return;
        this.favoriteData.isFavorite = !this.favoriteData.isFavorite;

        this.favoriteClick.emit(this.favoriteData);
    }
}
