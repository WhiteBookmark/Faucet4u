<template>
    <v-dialog :value="errorModal"
              max-width="290"
              @click:outside="close()"
              @keydown.esc="close()">
        <v-card>
            <v-card-title class="headline red--text">Error</v-card-title>
            <v-card-text class="font-weight-black black--text">
                Unfortunately an error occured, therefore your request was not proceed.<br />
                Please contact our support team via chat, support ticket or at <a :href="forumLink">forums</a> and provide the following error details:
                <br />
                <b class="red--text">{{errorMessage}}</b>
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
        name: "ErrorModal",
        data() {
            return {
                forumLink: this.$store.getters.faucetSettingValue('ForumLink')
            };
        },
        computed: {
            ...get(['errorModal', 'errorMessage'])
        },
        methods: {
            close: function () {
                store.set('errorModal', false);
            }
        }
    };
</script>
