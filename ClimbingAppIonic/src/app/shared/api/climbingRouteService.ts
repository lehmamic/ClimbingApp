import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ClimbingSite } from 'src/app/climbing-route/climbing-route.reducer';

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
    climbingRoute: ClimbingRouteMatch;
}

export interface ClimbingRouteMatch {
    id: string;
    name: string;
    description: string;
    grade: string;
    type: ClimbingRouteType;
    site: ClimbingSiteMatch;
    imageUri: string;
}

export interface ClimbingSiteMatch {
    id: string;
    name: string;
    description: string;
}

export interface ClimbingSiteResponse {
    id: string;
    name: string;
    description: string;
}

export interface CreateClimbingSiteRequest {
    name: string;
    description?: string;
}

export interface ClimbingRouteResponse {
    id: string;
    name: string;
    description: string;
    grade: string;
    type: ClimbingRouteType;
    imageUri: string;
}

export interface CreateClimbingRouteRequest {
    name: string;
    description?: string;
    grade: string;
    type: ClimbingRouteType;
    image: Image;
}

@Injectable()
export class ClimbingRouteService {
    constructor(private http: HttpClient) { }

    public getClimbingSites(): Observable<ClimbingSiteResponse[]> {
        return this.http.get<ClimbingSiteResponse[]>('http://localhost:5002/api/v1/sites');
    }

    public createClimbingSite(request: CreateClimbingSiteRequest): Observable<ClimbingSiteResponse> {
        return this.http.post<ClimbingSiteResponse>('http://localhost:5002/api/v1/sites', request);
    }

    public createClimbingRoute(siteId: string, request: CreateClimbingRouteRequest): Observable<ClimbingRouteResponse> {
        return this.http.post<ClimbingRouteResponse>(`http://localhost:5002/api/v1/sites/${siteId}/routes`, request);
    }

    public query(request: ImageRecognitionQueryRequest): Observable<ImageRecognitionQueryResponse> {
        return this.http.post<ImageRecognitionQueryResponse>('http://localhost:5002/api/v1/query', request);
    }
}
