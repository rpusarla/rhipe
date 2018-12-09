import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import Triangleservice = require("./triangle.service");
import TriangleService = Triangleservice.TriangleService;

@Component({
  selector: 'app-triangle',
  templateUrl: './triangle.component.html',
  styleUrls: ['./triangle.component.css']
})
export class TriangleComponent implements OnInit {
  @ViewChild("triangleCanvas") triangleCanvas: ElementRef;
  errors: string;

  triangleForm = this.fb.group({
    inputText: ['', Validators.required]
  });

  constructor(private fb: FormBuilder, private service: TriangleService) { }

  ngOnInit() {
    
  }
  drawTriangle(result) {
    var AB = result.base;
    var BC = result.side1;
    var AC = result.side2;

    let ctx: CanvasRenderingContext2D = this.triangleCanvas.nativeElement.getContext("2d");
    ctx.clearRect(0, 0, this.triangleCanvas.nativeElement.width, this.triangleCanvas.nativeElement.height);

    var A = [0, 0]; // starting coordinates
    var B = [0, AB];
    var C = [];

    // calculate third point
    C[1] = ((AB * AB) + (AC * AC) - (BC * BC)) / (2 * AB);
    C[0] = Math.sqrt(AC * AC - C[1] * C[1]);


    ctx.beginPath();
    ctx.moveTo(A[0], A[1]);
    ctx.lineTo(B[0], B[1]);
    ctx.lineTo(C[0], C[1]);
    ctx.fill();
  }

  onSubmit() {
    var inputText = this.triangleForm.get('inputText').value;
    this.errors = "";
    
    return this.service.getData(inputText)
      .subscribe(result => {
          this.drawTriangle(result);          
      },
      error => {
        this.errors = error.error;        
      });
  }

}
