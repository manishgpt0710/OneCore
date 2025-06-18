import { Component, OnInit, Input } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AgGridAngular, AgGridModule } from 'ag-grid-angular';
import { CommonModule } from '@angular/common';
import { ColDef, GridOptions } from 'ag-grid-community';

@Component({
  selector: 'app-dpa-grid',
  templateUrl: './dpa-grid.html',
  styleUrls: ['./dpa-grid.scss'],
  standalone: true,
  imports: [
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    CommonModule,
    AgGridModule,
    AgGridAngular
  ]
})
export class DpaGridComponent implements OnInit {
  @Input() rows!: any[];
  @Input() columns!: any[];

  gridOptions!: GridOptions;

  autoGroupColumnDef: ColDef = {
    headerName: "",
    field: "customerName",
    cellRendererParams: {
      suppressCount: true,
    },
  };

	ngOnInit(): void {
    this.gridOptions = {
      columnDefs: this.columns,
      rowData: this.rows,
      
      defaultColDef: {
        flex: 1,
        minWidth: 100,
        resizable: true,
        sortable: true,
        filter: true
      },
      
      pagination: true,
      paginationPageSize: 10,
      domLayout: 'autoHeight',
      
      autoGroupColumnDef: this.autoGroupColumnDef,
      treeDataChildrenField: 'children',
      groupDefaultExpanded: 0,
      treeData: true,
    };
  }
  
} 