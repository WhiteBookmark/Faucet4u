<template>
    <v-dialog :value="captchaModal"
              max-width="290" @
              click:outside="close()"
              @keydown.esc="close()">
        <v-card>
            <v-card-title class="headline">Captcha invalid</v-card-title>
            <v-card-text class="font-weight-black black--text">
                Captcha was invalid, please try again.
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="red" text @click="close()">Ok</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>

</template>

<script>
    import { get } from 'vuex-pathify';

    export default {
        name: "CaptchaModal",
        computed: {
            ...get(['captchaModal'])
        },
        methods: {
            close: function () {
                store.set('captchaModal', false);
            }
        }
    };
</script>
