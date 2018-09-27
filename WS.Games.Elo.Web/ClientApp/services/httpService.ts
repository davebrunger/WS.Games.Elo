import { SecurityService } from "./securityService";

export class HttpService {

    constructor(private readonly securityService: SecurityService) {

    }

    private performHttpRequest<T>(
        method: string,
        url: string,
        data?: { [key: string]: any },
        files?: FileList,
        successCallback?: (data: T) => void,
        errorCallback?: (status: number, responseText: string) => void
    ) {
        const formData = data || files ? new FormData() : undefined;

        if (data) {
            for (let key in data) {
                if (data.hasOwnProperty(key)) {
                    if (formData) {
                        formData.append(key, data[key]);
                    }
                }
            }
        }

        if (files) {
            for (let i = 0; i < files.length; i++) {
                if (formData) {
                    formData.append(files[i].name, files[i]);
                }
            }
        }

        const xhr = new XMLHttpRequest();
        xhr.open(method, url, true);

        var bearerToken = this.securityService.getBearerToken();

        if (bearerToken) {
            xhr.setRequestHeader("Authorization", `Bearer ${bearerToken}`);
        }

        xhr.onload = () => {
            if (xhr.status >= 200 && xhr.status < 300) {
                if (successCallback) {
                    const responseData = xhr.responseText ? JSON.parse(xhr.responseText) : null;
                    successCallback(responseData);
                }
            } else {
                if (xhr.status == 401) {
                    this.securityService.logout();
                }
                if (errorCallback) {
                    errorCallback(xhr.status, xhr.responseText);
                }
            }
        };
        xhr.send(formData);
    }

    public get<T>(
        url: string,
        successCallback: (data: T) => void,
        errorCallback?: (status: number, responseText: string) => void
    ) {
        this.performHttpRequest("GET", url, undefined, undefined, successCallback, errorCallback);
    }

    public post<T>(
        url: string,
        data?: { [key: string]: any },
        files?: FileList,
        successCallback?: (data: T) => void,
        errorCallback?: (status: number, responseText: string) => void
    ) {
        this.performHttpRequest("POST", url, data, files, successCallback, errorCallback);
    }
}