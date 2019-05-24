import { Plugins, CameraResultType, CameraSource, CameraPhoto, CameraOptions } from '@capacitor/core';
import { Observable, from } from 'rxjs';

export class Camera {
    getPhoto(options: CameraOptions): Observable<CameraPhoto> {
        return from(Plugins.Camera.getPhoto(options));
    }
}