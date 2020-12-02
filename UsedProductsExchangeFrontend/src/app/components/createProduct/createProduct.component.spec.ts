import { ComponentFixture, TestBed } from '@angular/core/testing';

import { createProductComponent } from './createProduct.component';

describe('createProductComponent', () => {
  let component: createProductComponent;
  let fixture: ComponentFixture<createProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ createProductComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(createProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
