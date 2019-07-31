<template>
    <v-dialog :value="successModal"
              max-width="290"
              @click:outside="close()"
              @keydown.esc="close()">
        <v-card>
            <v-card-title class="headline green--text">Success</v-card-title>
            <v-card-text class="font-weight-black black--text">
                Operation was a success.
                <br />
                <b class="green--text">{{ successMessage }}</b>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="green" text @click="close()">Ok</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
    import { get } from 'vuex-pathify';

    export default {
        name: "SuccessModal",
        computed: {
            ...get(['successModal', 'successMessage'])
        },
        methods: {
            close: function () {
                store.set('successModal', false);
            }
        }
    };
</script>
