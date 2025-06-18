import { Component } from "@angular/core";
import { ICellRendererParams } from "ag-grid-community";

@Component({
    selector: 'buy-button-renderer',
    template: `
        <img [src]="imageUrl" style="height: 40px;" (click)="toggle()">
    `
})
export class CellBuyButtonRenderer {
    cellValue = false;

    get imageUrl(): string {
        return this.cellValue ? '../assets/images/buy_logo.png' : '../assets/images/bro_logo.gif'
    }

    agInit(params: ICellRendererParams): void {
        this.cellValue = params.value;
    }

    toggle(): void {
        this.cellValue = !this.cellValue;
    }
}