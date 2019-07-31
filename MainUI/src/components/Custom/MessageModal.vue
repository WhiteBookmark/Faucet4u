<template>
    <v-dialog :value="messageModal"
              max-width="290"
              @click:outside="close()"
              @keydown.esc="close()">
        <v-card>
            <v-card-title class="headline cyan--text">Message</v-card-title>
            <v-card-text class="font-weight-black black--text">
                {{ messageModalMessage }}
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="cyan" text @click="close()">Ok</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
    import { get } from 'vuex-pathify';

    export default {
        name: "MessageModal",
        computed: {
            ...get(['messageModal', 'messageModalMessage'])
        },
        methods: {
            close: function () {
                store.set('messageModal', false);
            }
        }
    };
</script>
