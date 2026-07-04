import { Component, OnInit, Input, } from '@angular/core';
  
@Component({
  selector: 'main-tree-menu',
  templateUrl: './tree-menu.component.html',
  styleUrls: ['./tree-menu.component.scss']
})
export class MainTreeMenuComponent implements OnInit {
  @Input('menuItems') menuItems: any[];
  @Input('hasChild') hasChild: boolean=false;
  

     

  constructor(  
  ) {

  

  }

  ngOnInit() { 
  }

   
 
 

}
