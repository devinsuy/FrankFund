import axios from "axios";

const API_URL = "/api/session/create&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0";

class AuthService {
    login(usernameoremail, password) {
        return axios
            //  + "signin", { usernameoremail, password }
            .post(API_URL)
            .then((response) => {
                if (response.data.accessToken) {
                    localStorage.setItem("user", JSON.stringify(response.data));
                }

                return response.data;
            });
    }

    logout() {
        localStorage.removeItem("user");
    }

    // Already implemented in createuseraccount.component ??
    register(username, email, password) {
        return axios.post(API_URL + "signup", {
            username,
            email,
            password,
        });
    }
}

export default new AuthService();