import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

export interface ImageRecognitionQueryRequest {
    image: Image;
}

export interface Image {
    base64: string;
}

export interface ImageRecognitionQueryResponse {
    results: QueryResultEntry[];
}

export interface QueryResultEntry {
    targetId: string;
    displayName: string;
    description: string;
    score: number;
}

@Injectable()
export class ImageRecognitionService {
    constructor(private http: HttpClient) { }

    public query(request: ImageRecognitionQueryRequest): Observable<ImageRecognitionQueryResponse> {
        return this.http.post<ImageRecognitionQueryResponse>('http://localhost:5001/api/v1/query', request);
    }
}
