import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Subject, Observable } from 'rxjs';

export class Config{
    static defaultOptions: {
        headers?: HttpHeaders | {
            [header: string]: string | string[];
        };
        observe?: 'body';
        params?: HttpParams | {
            [param: string]: string | string[];
        };
        reportProgress?: boolean;
        withCredentials?: boolean;
    };

    static onError = new Subject<{error: any, caught: Observable<any>}>();
    static authError = new Subject<any>();
}