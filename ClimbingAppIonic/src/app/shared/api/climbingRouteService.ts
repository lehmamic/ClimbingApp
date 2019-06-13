import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

export interface ImageRecognitionQueryRequest {
    image: Image;
}

export interface Image {
    base64: string;
}

export type ImageRecognitionQueryResult =
    'Match' |
    'NoMatch';

export type ClimbingRouteType =
    'SportClimbing' |
    'Bouldering';

export interface ImageRecognitionQueryResponse {
    result: ImageRecognitionQueryResult;
    climbingSite: ClimbingSiteMatch;
}

export interface ClimbingRouteMatch {
    id: string;
    name: string;
    description: string;
    grade: string;
    type: ClimbingRouteType;
}

export interface ClimbingSiteMatch {
    id: string;
    name: string;
    description: string;
    route: ClimbingRouteMatch;
}

@Injectable()
export class ClimbingRouteService {
    constructor(private http: HttpClient) { }

    public query(request: ImageRecognitionQueryRequest): Observable<ImageRecognitionQueryResponse> {
        return this.http.post<ImageRecognitionQueryResponse>('http://localhost:5002/api/v1/query', request);
    }
}
