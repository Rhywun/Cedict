import {NgModule} from "@angular/core";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatTableModule} from "@angular/material/table";
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import {MatGridListModule} from "@angular/material/grid-list";
import {MatCheckboxModule} from "@angular/material/checkbox";

@NgModule({
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatInputModule,
    MatButtonModule,
    MatGridListModule,
    MatCheckboxModule
  ],
  exports: [
    MatTableModule,
    MatPaginatorModule,
    MatInputModule,
    MatButtonModule,
    MatGridListModule,
    MatCheckboxModule
  ]
})
export class AngularMaterialModule {
}
