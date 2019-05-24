import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { Camera, CameraComponent, cameraFactory } from './camera';
import { Platform, ModalController } from '@ionic/angular';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MaterialFileInputModule } from 'ngx-material-file-input';

@NgModule({
  declarations: [
    CameraComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MaterialFileInputModule,
  ],
  providers: [
    { provide: Camera, useFactory: cameraFactory, deps: [Platform, ModalController]},
  ],
  exports: [
  ],
  entryComponents: [
    CameraComponent
  ],
})
export class NativeModule { }
