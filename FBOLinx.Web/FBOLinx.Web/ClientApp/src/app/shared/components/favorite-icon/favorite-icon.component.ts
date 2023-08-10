import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-favorite-icon',
  templateUrl: './favorite-icon.component.html',
  styleUrls: ['./favorite-icon.component.scss']
})
export class FavoriteIconComponent implements OnInit {
    @Input() isFavorite: boolean = false;
    @Output() favoriteClick = new EventEmitter<boolean>();

    constructor() { }

    ngOnInit() {
    }
    toogleFavorite(): void{
        this.isFavorite = !this.isFavorite;
        this.favoriteClick.emit(this.isFavorite);
    }
}
