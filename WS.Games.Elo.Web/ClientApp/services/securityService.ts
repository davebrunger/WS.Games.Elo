import { HttpService } from "./httpService";

export class SecurityService {

    private currentUser?: string;

    constructor(private readonly onLoggedIn: (currentUser: string) => void, private readonly onLoggedOut: () => void) {
        this.currentUser = undefined;
    }

    public login(username: string, password: string, callback: (loggedIn: boolean) => void) {
        localStorage.removeItem("token");
        var httpService = new HttpService(this);
        httpService.post<{ token: string }>("/api/users/login", { "username": username, "password": password }, undefined,
            data => {
                localStorage.setItem("token", data.token);
                this.currentUser = username;
                this.onLoggedIn(username);
                callback(true);
            },
            () => {
                callback(false);
            });
    }

    public getBearerToken(): any {
        return localStorage.getItem("token");
    }

    public logout(): any {
        localStorage.removeItem("token");
        this.currentUser = undefined;
        this.onLoggedOut();
    }

    public getCurrentUser(callback: (curentUser?: string) => void) {
        if (this.currentUser) {
            callback(this.currentUser);
            return;
        }
        if (!this.getBearerToken()) {
            callback(undefined);
            return;
        }
        var httpService = new HttpService(this);
        httpService.get<{ name: string }>("/api/users/current", data => {
            this.currentUser = data.name;
            callback(data.name);
        }, () => {
            this.logout();
        });
    }
}