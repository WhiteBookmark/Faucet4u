//Importing store here is neccessary, universal store reference doesn't work in this file
import { store } from '@/scripts/store';

const mixins = {

    methods: {
        bitcoinFormat: amount => parseFloat(amount).toFixed(8),
        bitcoinAgGridFormat: amount => parseFloat(amount.value).toFixed(8),
        buildErrorMessage: function (error) {
            if (error.isAxiosError) {
                let concatMessages = '';
                Object.entries(error.response.data.errors)
                    .forEach(([key, value]) => concatMessages += value);
                return concatMessages;
            }
            return error.toString();
        },
        executeInBackground: async function (operation, successUI = true, errorUI = true) {
            try {
                store.set("isLoading", true);
                const response = await operation();
                if (successUI) {
                    store.set("successMessage", response.data.message);
                    store.set("successModal", true);
                }
            }
            catch (error) {

                const errorMessage = this.buildErrorMessage(error);

                if (errorUI) {
                    store.set("errorMessage", errorMessage);
                    store.set("errorModal", true);
                }
                else {
                    console.log(errorMessage);
                }
            }
            finally {
                store.set("isLoading", false);
            }
        }
    }

};

export default mixins;