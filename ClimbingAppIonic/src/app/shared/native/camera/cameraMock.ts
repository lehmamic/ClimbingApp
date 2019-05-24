import { Camera } from './camera';
import { CameraOptions, CameraPhoto } from '@capacitor/core';
import { Observable, from } from 'rxjs';
import { ModalController } from '@ionic/angular';
import { CameraComponent } from './camera.component';

export class CameraMock extends Camera {
    constructor(private modalController: ModalController) {
        super();
    }

    public getPhoto(options: CameraOptions): Observable<CameraPhoto> {
        return from(this.uploadPhoto());
    }

    private async uploadPhoto(): Promise<CameraPhoto> {
        const modal = await this.modalController.create({component: CameraComponent});
        modal.present();
        const { data } = await modal.onDidDismiss();

        return data;
    }
}