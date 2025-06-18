import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import { ModuleRegistry, AllCommunityModule, ClientSideRowModelModule } from 'ag-grid-community';
import { TreeDataModule } from "ag-grid-enterprise";

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));

ModuleRegistry.registerModules([ AllCommunityModule, ClientSideRowModelModule, TreeDataModule ]);