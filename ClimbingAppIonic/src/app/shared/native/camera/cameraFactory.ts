import { Camera } from './camera';
import { Platform, ModalController } from '@ionic/angular';
import { CameraMock } from './cameraMock';

export function cameraFactory(platform: Platform, modalController: ModalController): Camera {
    if (platform.is('capacitor')) {
        return new Camera();
    } else {
        return new CameraMock(modalController);
    }
}