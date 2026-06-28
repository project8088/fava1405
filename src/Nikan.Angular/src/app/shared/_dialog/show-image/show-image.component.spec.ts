import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowImageDialogComponent } from './show-image.component';

describe('ShowImageDialogComponent', () => {
  let component: ShowImageDialogComponent;
  let fixture: ComponentFixture<ShowImageDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowImageDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowImageDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
