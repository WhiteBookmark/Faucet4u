<template>

    <v-container fluid text-center justify-center align-center pa-0>
        <v-layout>
            <v-flex xs1>
                <!--<skyscrapper-banner></skyscrapper-banner>-->
            </v-flex>
            <v-flex>
                <v-data-table :headers="headers"
                              :items="ptpData"
                              sort-by="Creation"
                              sort-desc
                              class="elevation-24">
                    <template v-slot:top>
                        <v-toolbar>
                            <v-toolbar-title>PTP Advertisements</v-toolbar-title>
                            <v-divider class="mx-4"
                                       inset
                                       vertical></v-divider>
                            <v-spacer></v-spacer>
                            <v-dialog v-model="dialog" max-width="500px">
                                <template v-slot:activator="{ on }">
                                    <v-btn color="orange" outlined large ripple elevation="24" v-on="on">Add new advertisement</v-btn>
                                </template>
                                <v-card>
                                    <v-card-title>
                                        <span class="headline">New advertisement</span>
                                    </v-card-title>

                                    <v-card-text>
                                        <v-container grid-list-md>
                                            <v-layout column class="black--text font-weight-black">
                                                <v-flex>
                                                    PTP Credits: {{ ptpCredit }}
                                                    <v-divider class="mx-4"
                                                               inset
                                                               vertical></v-divider>
                                                    PTP Day Credits: {{ ptpDayCredit }}
                                                </v-flex>
                                                <v-flex>
                                                    <p v-if="!isTimeBased.value">1 Credit = 1 Hit</p>
                                                    <p v-else>1 Credit = 1 Day</p>
                                                </v-flex>
                                                <v-flex>
                                                    <v-select v-model="isTimeBased" :options="type" :clearable="false"></v-select>
                                                </v-flex>
                                                <v-flex>
                                                    <v-text-field v-validate="'required'"
                                                                  name="link"
                                                                  v-model.trim="link"
                                                                  filled
                                                                  label="Link"
                                                                  :error-messages="errors.collect('link')"></v-text-field>
                                                </v-flex>
                                                <v-flex>
                                                    <v-text-field key="hits-credit"
                                                                  filled
                                                                  label="Credits"
                                                                  v-if="!isTimeBased.value"
                                                                  v-validate="`required|min_value:0|max_value:${ptpCredit}`"
                                                                  name="credit"
                                                                  v-model.trim="credit"
                                                                  type="number"
                                                                  :error-messages="errors.collect('credit')"></v-text-field>
                                                    <v-text-field key="time-credit"
                                                                  filled
                                                                  label="Day Credits"
                                                                  v-validate="`required|min_value:0|max_value:${ptpDayCredit}`"
                                                                  name="credit"
                                                                  v-model.trim="credit"
                                                                  type="number"
                                                                  :error-messages="errors.collect('credit')"
                                                                  v-else></v-text-field>
                                                </v-flex>
                                            </v-layout>
                                        </v-container>
                                    </v-card-text>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="orange" text @click="close">Cancel</v-btn>
                                        <v-btn color="orange" text @click="save">Save</v-btn>
                                    </v-card-actions>
                                </v-card>
                            </v-dialog>
                        </v-toolbar>
                    </template>
                    <template v-slot:body="props">
                        <tr v-for="index in props.items">
                            <td v-for="header in headers">
                                {{ index[header.value]}}
                            </td>
                        </tr>
                    </template>
                    <template v-slot:item.action="{ item }">
                        <v-icon small
                                class="mr-2"
                                @click="editItem(item)">
                            mdi-pencil
                        </v-icon>
                        <v-icon small
                                @click="deleteItem(item)">
                            mdi-delete
                        </v-icon>
                    </template>
                    <template v-slot:no-data>
                        <v-btn color="orange" outlined ripple elevation="24" @click="initialize">Load data</v-btn>
                    </template>
                </v-data-table>
            </v-flex>
            <v-flex xs1>
                <!--<skyscrapper-banner></skyscrapper-banner>-->
            </v-flex>
            <v-flex xs1> </v-flex>
        </v-layout>

        <v-layout>
            <v-flex>
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
            </v-flex>
            <v-flex>
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
            </v-flex>
        </v-layout>

    </v-container>

</template>

<script>

    export default {
        data() {
            return {
                recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
                reference: null,
                link: null,
                credit: null,
                doesntExist: false,
                isTimeBased: { label: 'Hits Based', value: false },
                type: [{ label: 'Hits Based', value: false }, { label: 'Time Based', value: true }],
                ptpData: [],
                dialog: false,
                headers: [
                    { text: 'Reference', align: 'left', sortable: false, value: 'Reference' },
                    { text: 'Created', value: 'Creation' },
                    { text: 'Link', value: 'Link' },
                    { text: 'Is this time based ?', value: 'IsTimeBased' },
                    { text: 'Credit', value: 'Credit' },
                    { text: 'Hits Received', value: 'HitsReceived' }
                ],
                editedIndex: -1,
                editedItem: {
                    Link: 'http://faucet4all.com',
                    IsTimeBased: false,
                    Credit: 0
                },
                defaultItem: {
                    Link: 'http://faucet4all.com',
                    IsTimeBased: false,
                    Credit: 0
                }
            }
        },
        methods: {

            //updatePTPOrder: async function () {
            //    this.doesntExist = false;
            //    var hasAny = false;

            //    this.$validator.validateAll({ reference: this.reference, link: this.link, credit: this.credit })
            //        .then(async result => {
            //            if (result === true) {
            //                for (var i = 0; i < this.$store.state.reactivePTPOrderData.rows.length; i++) {
            //                    if (
            //                        this.reference ===
            //                        this.$store.state.reactivePTPOrderData.rows[i].reference &&
            //                        this.$store.state.reactivePTPOrderData.rows[i].isTimeBased ===
            //                        "false"
            //                    ) {
            //                        hasAny = true;
            //                        break;
            //                    }
            //                }

            //                if (hasAny === true) {
            //                    this.$socket.emit(
            //                        "updatingAdvertisingOrder",
            //                        JSON.stringify({
            //                            sessionId: this.$cookie.get("sessionId"),
            //                            reference: this.reference,
            //                            name: "PTP",
            //                            link: this.modifyLink,
            //                            isTimeBasedInput: "0",
            //                            creditInput: this.credit
            //                        })
            //                    );
            //                } else if (hasAny === false) {
            //                    this.doesntExist = true;
            //                }
            //            }
            //        });
            //},

            //updatePTPDayOrder: function () {
            //    this.doesntExist = false;
            //    var hasAny = false;

            //    this.$validator
            //        .validateAll({
            //            reference: this.reference,
            //            link: this.link,
            //            credit: this.credit
            //        })
            //        .then(result => {
            //            if (result === true) {
            //                for (var i = 0; i < this.$store.state.reactivePTPOrderData.rows.length; i++) {
            //                    if (
            //                        this.reference ===
            //                        this.$store.state.reactivePTPOrderData.rows[i].reference &&
            //                        this.isTimeBased === "true"
            //                    ) {
            //                        hasAny = true;
            //                        break;
            //                    }
            //                }

            //                if (hasAny === true) {
            //                    this.$socket.emit(
            //                        "updatingAdvertisingOrder",
            //                        JSON.stringify({
            //                            sessionId: this.$cookie.get("sessionId"),
            //                            reference: this.reference,
            //                            name: "PTP",
            //                            link: this.modifyLink,
            //                            isTimeBasedInput: "1",
            //                            creditInput: 0
            //                        })
            //                    );
            //                } else if (hasAny === false) {
            //                    this.doesntExist = true;
            //                }
            //            }
            //        });
            //},

            editItem(item) {
                this.editedIndex = this.desserts.indexOf(item)
                this.editedItem = Object.assign({}, item)
                this.dialog = true
            },

            deleteItem(item) {
                const index = this.desserts.indexOf(item)
                confirm('Are you sure you want to delete this item?') && this.desserts.splice(index, 1)
            },

            close() {
                this.dialog = false
                setTimeout(() => {
                    this.editedItem = Object.assign({}, this.defaultItem)
                    this.editedIndex = -1
                }, 300)
            },

            save: async function () {

                try {

                    const result = await this.$validator.validateAll({ link: this.link, credit: this.credit });
                    if (!result) return;

                    const response = await this.axios.post(process.env.VUE_APP_API_AdvertiserPanel, {
                        sessionId: this.$cookie.get("sessionId"),
                        name: "PTP",
                        link: this.link,
                        isTimeBased: this.isTimeBased.value,
                        creditInput: this.credit
                    });

                    this.initialize();
                    this.$store.commit("successMessage", response.data.message);
                    this.$store.commit("successModal", true);
                }
                catch (error) {
                    if (error.isAxiosError) {
                        var concatMessages = '';
                        Object.entries(error.response.data.errors).forEach(([key, value]) => concatMessages += value);
                        this.$store.commit("errorMessage", concatMessages);
                        this.$store.commit("errorModal", true);
                    }
                    else {
                        this.$store.commit("errorMessage", error.toString());
                        this.$store.commit("errorModal", true);
                    }
                }
                finally {
                    this.close();
                }
            },
            initialize: async function () {
                try {
                    const response = await this.axios.get(process.env.VUE_APP_API_AdvertiserPanel, { params: { sessionId: this.$cookie.get('sessionId'), name: 'PTP' } });
                    this.ptpData = response.data;
                }
                catch (error) {
                    if (error.isAxiosError) {
                        var concatMessages = '';
                        Object.entries(error.response.data.errors).forEach(([key, value]) => concatMessages += value);
                        this.$store.commit("errorMessage", concatMessages);
                        this.$store.commit("errorModal", true);
                    }
                    else {
                        this.$store.commit("errorMessage", error.toString());
                        this.$store.commit("errorModal", true);
                    }
                }
            }
        },
        created() {
            this.initialize();
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
            },
            //ptpCredit: this.sync('userData[0].PTPDayCredit'),
            ptpCredit: {
                get() {
                    return this.$store.state.userData[0].PTPCredit;
                }
            },
            ptpDayCredit: {
                get() {
                    return this.$store.state.userData[0].PTPDayCredit;
                }
            }

        },
        watch: {
            dialog(val) {
                val || this.close()
            },
        }
    };
</script>
