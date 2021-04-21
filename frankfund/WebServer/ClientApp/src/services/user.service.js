// JavaScript source code - define a service for accessing data in user.service.js
import axios from 'axios';
import authHeader from './auth-header';

const API_URL = '/api/account/accID=1&apikey=f2f1178729cb2e1c9188ed847066743c4e843a21';

// Add a HTTP header with the help of authHeader() function when requesting authorized resource.
class UserService {
    getPublicContent() {
        return axios.get(API_URL);
    }

    getUserBoard() {
        return axios.get(API_URL + 'user', { headers: authHeader() });
    }
}

export default new UserService();