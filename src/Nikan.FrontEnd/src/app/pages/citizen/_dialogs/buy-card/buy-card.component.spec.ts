import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuyCardDialogComponent } from './buy-card.component';

describe('BuyCardDialogComponent', () => {
  let component: BuyCardDialogComponent;
  let fixture: ComponentFixture<BuyCardDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BuyCardDialogComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BuyCardDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
