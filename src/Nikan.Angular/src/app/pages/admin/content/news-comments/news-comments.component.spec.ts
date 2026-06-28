import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminNewsCommentsComponent } from './news-comments.component';

describe('AdminNewsCommentsComponent', () => {
  let component: AdminNewsCommentsComponent;
  let fixture: ComponentFixture<AdminNewsCommentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminNewsCommentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminNewsCommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
