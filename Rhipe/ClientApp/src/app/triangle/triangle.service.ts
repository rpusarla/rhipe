import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class TriangleService {

  constructor(private http: HttpClient) { }

  getData(inputData) {
    return this.http.get("http://localhost:53910/api/TextValidation/IsInputTextValid?inputText="+inputData);
  }

}
