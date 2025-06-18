import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { DpaGridComponent } from "../widgets/dpa-grid/dpa-grid.component";
import { AgGridModule } from 'ag-grid-angular';


import { ColDef } from "ag-grid-community";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  standalone: true,
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatTableModule,
    MatIconModule,
    CommonModule,
    MatProgressBarModule,
    DpaGridComponent,
    CommonModule,
        AgGridModule
]
})
export class DashboardComponent {
  userEmail = localStorage.getItem('userEmail') || 'user@example.com';
  displayedColumns: string[] = [];
  dataSource: any[] = [];
  clientName = 'Client Name';
  isLoading = false;

  columns: ColDef[] =  [
    { field: 'id', width: 50, headerName: 'ID', sortable: true, filter: true },
    { field: 'customerName', width: 300, headerName: 'Customer Name', sortable: true, filter: true },
    { field: 'advance', width: 150, headerName: 'Advance', sortable: true, filter: true },
    { field: 'retention', width: 150, headerName: 'Retention', sortable: true, filter: true },
    { field: 'pbgAmount', width: 150, headerName: 'PBG Amount', sortable: true, filter: true },
    { field: 'againstGrn', width: 150, headerName: 'Against GRN', sortable: true, filter: true },
    { field: 'balanceDue', width: 150, headerName: 'Balance Due', sortable: true, filter: true },
    { field: 'range30', width: 150, headerName: '0-30', sortable: true, filter: true },
    { field: 'rane60', width: 150, headerName: '30-60', sortable: true, filter: true },
    { field: 'rane90', width: 150, headerName: '60-90', sortable: true, filter: true },
    { field: 'total', width: 150, headerName: 'Total', sortable: true, filter: true },
  ];

  rowData = data;

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.isLoading = true;
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const text = e.target.result;
        this.parseCSV(text);
        this.isLoading = false;
      };
      reader.readAsText(file);
    }
  }

  parseCSV(csv: string) {
    const lines = csv.split('\n').filter(line => line.trim() !== '');
    if (lines.length === 0) return;
    this.displayedColumns = lines[0].split(',').map(h => h.trim());
    this.dataSource = lines.slice(1).map(line => {
      const values = line.split(',');
      const obj: any = {};
      this.displayedColumns.forEach((col, i) => {
        obj[col] = values[i]?.trim() || '';
      });
      return obj;
    });
  }

  logout() {
    this.isLoading = true;
    // TODO: Implement logout logic
    setTimeout(() => {
      window.location.href = '/login';
    }, 500);
  }
}

const data = [
  { id: 1, customerName: 'Customer A', range30: 100, rane60: 200, rane90: 300, total: 600,
    children: [
      { poNumber: 1, customerName: 'POA123', range30: 50, rane60: 100, rane90: 150, total: 300,
        children: [
          { invNumber: 101, customerName: '101', range30: 20, rane60: 40, rane90: 60, total: 120 },
        ]
      },
      { poNumber: 2, customerName: 'POA456', range30: 30, rane60: 60, rane90: 90, total: 180 },
      { poNumber: 3, customerName: 'POA789', range30: 20, rane60: 40, rane90: 60, total: 120 }
    ]
  },
  { id: 2, customerName: 'Customer B', range30: 150, rane60: 250, rane90: 350, total: 750,
    children: [
      { poNumber: 1, customerName: 'POA123', range30: 70, rane60: 120, rane90: 160, total: 350 },
      { poNumber: 2, customerName: 'POA456', range30: 40, rane60: 80, rane90: 120, total: 240 },
      { poNumber: 3, customerName: 'POA789', range30: 30, rane60: 50, rane90: 80, total: 160 }
    ]
   },
  { id: 3, customerName: 'Customer C', range30: 200, rane60: 300, rane90: 400, total: 900,
    children: [
      { poNumber: 1, customerName: 'POA123', range30: 70, rane60: 120, rane90: 160, total: 350 },
      { poNumber: 2, customerName: 'POA456', range30: 40, rane60: 80, rane90: 120, total: 240 },
      { poNumber: 3, customerName: 'POA789', range30: 30, rane60: 50, rane90: 80, total: 160 }
    ]
   },
  { id: 4, customerName: 'Customer D', range30: 250, rane60: 350, rane90: 450, total: 1050,
    children: [
      { poNumber: 1, customerName: 'POA123', range30: 70, rane60: 120, rane90: 160, total: 350 },
      { poNumber: 2, customerName: 'POA456', range30: 40, rane60: 80, rane90: 120, total: 240 },
      { poNumber: 3, customerName: 'POA789', range30: 30, rane60: 50, rane90: 80, total: 160 }
    ]
   },
];