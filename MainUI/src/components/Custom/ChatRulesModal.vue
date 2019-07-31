<template>
    <v-dialog :value="chatRulesModal"
              max-width="400"
              @click:outside="close()"
              @keydown.esc="close()">
        <v-card>
            <v-card-title class="headline orange--text">Chat Rules</v-card-title>
            <v-card-text class="font-weight-black black--text text-left overflow-y-auto" style="height: 300px;">
                <v-list class="pa-0">
                    <v-list-item-group color="orange">

                        <v-list-item>
                            <v-list-item-content>
                                We may, at any time, and at our sole discretion, modify these chat
                                rules, with or without prior notice. Any such modification will be
                                effective immediately upon public posting on this page. Your
                                continued use of our chat box following any such modification
                                constitutes your acceptance of these modified rules.
                            </v-list-item-content>
                        </v-list-item>

                        <v-list-item>
                            <v-list-item-content>
                                Faucet4all asks all users to abide by the chat rules and policies.
                                This is not to restrict your rights to free speech but to create a
                                pleasant chatting environment for yourself everyone.
                            </v-list-item-content>
                        </v-list-item>

                        <v-list-item>
                            <v-list-item-content>
                                No inappropriate language, keep it clean.
                            </v-list-item-content>
                        </v-list-item>

                        <v-list-item>
                            <v-list-item-content>
                                Do not post any link in the chatbox. Doing so may get you banned
                                from the usage of chat box.
                            </v-list-item-content>
                        </v-list-item>

                        <v-list-item>
                            <v-list-item-content>
                                Racism/sexism/homophobia or hate towards Faucet4all staff members or
                                other users will simply not be tolerated. This includes beng toxic
                                and continuing to be negative in general. Have respect for everyone,
                                we're not a dumping ground.
                            </v-list-item-content>
                        </v-list-item>

                        <v-list-item>
                            <v-list-item-content>
                                Breaching privacy. To ensure everyone stays safe in Faucet4all no
                                one may share information that can be used to personally identify
                                themselves or others. This may include a photo, full name,
                                wallet/email/postal addresses, phone number, Facebook/Twitter, etc.
                                personal pages or other contact information. If you feel the need to
                                expose yourself to risk, then do it at your own risk.
                            </v-list-item-content>
                        </v-list-item>

                        <v-list-item>
                            <v-list-item-content>
                                No trolling. That includes (but not limited to): deliberately
                                provoking another user via cynical or sarcastic remarks, baiting
                                others to send an angry or offensive response to you and so on. It
                                is up to a moderator to decide what other behavior is considered
                                trolling.
                            </v-list-item-content>
                        </v-list-item>

                        <v-list-item>
                            <v-list-item-content>
                                No general spam. That includes (but not limited to): sending the
                                same message over and over again, pointless messages meant to annoy
                                others and/or posted just to qualify for chat rains,
                                random/meaningless messages, etc.
                            </v-list-item-content>
                        </v-list-item>

                        <v-list-item>
                            <v-list-item-content>
                                Do not use capitals. It is not necessary and many users interpret
                                this as yelling or shouting.
                            </v-list-item-content>
                        </v-list-item>

                        <v-list-item>
                            <v-list-item-content>
                                If a moderator tells you to refrain from doing something in the
                                chat, please respect the moderator's wish and stop. If you think a
                                moderator is abusing their power, contact an administrator.
                            </v-list-item-content>
                        </v-list-item>

                    </v-list-item-group>
                </v-list>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="orange" text @click="close()">Ok</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>

</template>

<script>
    import { get } from 'vuex-pathify';

    export default {
        name: "ChatRulesModal",
        computed: {
            ...get(['chatRulesModal'])
        },
        methods: {
            close: function () {
                store.set('chatRulesModal', false);
            }
        }
    };
</script>
