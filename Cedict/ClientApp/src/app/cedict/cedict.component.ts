import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";
import {Entry} from "./entry";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {HttpClient, HttpParams} from "@angular/common/http";
import {MatPaginator, PageEvent} from "@angular/material/paginator";

@Component({
  selector: 'app-cedict',
  templateUrl: './cedict.component.html',
  styleUrls: ['./cedict.component.css']
})
export class CedictComponent implements OnInit {
  public displayedColumns: string[] = ['traditional', 'simplified', 'pinyin', 'english']
  public entries: MatTableDataSource<Entry>;

  formModel: FormGroup;
  http: HttpClient;
  baseUrl: string;

  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;

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
    let type = this.formModel.get('characterSearch').value;
    let ignoreTones = this.formModel.get('ignoreTones').value;
    let url = this.baseUrl + "api/entries"; // ?query=${searchString}&type=${type}`;
    console.log(url);
    let params = new HttpParams()
      .set("query", searchString)
      .set("type", type)
      .set("ignoreTones", ignoreTones)
      .set("pageIndex", event.pageIndex.toString())
      .set("pageSize", event.pageSize.toString());
    this.http.get<any>(url, {params})
      .subscribe(result => {
        this.paginator.length = result.totalCount;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;
        this.entries = new MatTableDataSource<Entry>(result.data);
      }, error => console.error(error));
  }

  onSubmit() {
    console.log(this.formModel.value);
    if (this.formModel.valid) {
      let pageEvent = new PageEvent();
      pageEvent.pageIndex = 0;
      pageEvent.pageSize = 10;
      this.getData(pageEvent);
    }
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
