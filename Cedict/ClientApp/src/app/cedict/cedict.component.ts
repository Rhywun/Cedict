import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";
import {Entry} from "./entry";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {HttpClient, HttpParams} from "@angular/common/http";
import {MatPaginator, PageEvent} from "@angular/material/paginator";
import {MatSort} from "@angular/material/sort";

@Component({
  selector: 'app-cedict',
  templateUrl: './cedict.component.html',
  styleUrls: ['./cedict.component.css']
})
export class CedictComponent implements OnInit {
  public displayedColumns: string[] = ['traditional', 'simplified', 'pinyin', 'english']
  public entries: MatTableDataSource<Entry>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "pinyin";
  public defaultSortOrder: string = "asc";

  formModel: FormGroup;
  http: HttpClient;
  baseUrl: string;

  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: false}) sort: MatSort;

  constructor(fb: FormBuilder, http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.formModel = fb.group({
      query: ['', Validators.required],
      characterSearch: false,
      ignoreTones: false,
    });
    this.http = http;
    this.baseUrl = baseUrl;
  }

  getData(event: PageEvent) {
    let searchString = this.formModel.get('query').value;
    let characterSearch = this.formModel.get('characterSearch').value;
    let ignoreTones = this.formModel.get('ignoreTones').value;
    let url = this.baseUrl + "api/entries";
    let params = new HttpParams()
      .set("query", searchString)
      .set("characterSearch", characterSearch)
      .set("ignoreTones", ignoreTones)
      .set("pageIndex", event.pageIndex.toString())
      .set("pageSize", event.pageSize.toString())
      .set("sortColumn", (this.sort) ? this.sort.active : this.defaultSortColumn)
      .set("sortOrder", (this.sort) ? this.sort.direction : this.defaultSortOrder);
    this.http.get<any>(url, {params})
      .subscribe(result => {
        this.paginator.length = result.totalCount;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;
        this.entries = new MatTableDataSource<Entry>(result.data);
      }, error => console.error(error));
  }

  onSubmit() {
    // console.log(this.formModel.value);
    if (this.formModel.valid) {
      this.loadData();
    }
  }

  loadData() {
      let pageEvent = new PageEvent();
      pageEvent.pageIndex = this.defaultPageIndex;
      pageEvent.pageSize = this.defaultPageSize;
      this.getData(pageEvent);
  }

  ngOnInit() {
    // let url = this.baseUrl + "api/entries";
    // this.http.get<any>(url).subscribe(result => {
    //   this.paginator.length = result.totalCount;
    //   this.paginator.pageIndex = result.pageIndex;
    //   this.paginator.pageSize = result.pageSize;
    //   this.entries = new MatTableDataSource<Entry>(result.data);
    // }, error => console.error(error));
  }

}
