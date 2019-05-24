import { Component, OnInit, OnDestroy } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FileValidator, FileInput } from 'ngx-material-file-input';
import { CameraPhoto } from '@capacitor/core';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-camera',
  templateUrl: './camera.component.html',
  styleUrls: ['./camera.component.scss'],
})
export class CameraComponent implements OnInit, OnDestroy {
  public form: FormGroup;

  // 100 MB
  private readonly maxSize = 104857600;
  private readonly notifier$ = new Subject();
  private photo: CameraPhoto;

  constructor(private modalController: ModalController, private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      requiredfile: [
        { value: undefined, disabled: false },
        [
          Validators.required,
          FileValidator.maxContentSize(this.maxSize)
        ]
      ],
    });
  }

  public get imageUrl(): string {
    if (!!this.photo) {
      return `data:image/jpg;base64,${this.photo.base64String}`;
    } else {
      return '';
    }
  }

  ngOnInit() {
    this.form.get('requiredfile').valueChanges
      .pipe(
        takeUntil(this.notifier$),
      )
      .subscribe(value => {
        const reader = new FileReader();
        reader.readAsDataURL(value.files[0]);
        reader.onload = () => {
          this.photo = {
            format: 'jpeg',
            base64String: (<string>reader.result).split(',')[1],
          };
        };
      });
  }

  ngOnDestroy(): void {
    this.notifier$.next();
  }

  public submit() {
        this.modalController.dismiss(this.photo);
  }
}
