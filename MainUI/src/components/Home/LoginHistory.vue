<template>
    <div>
        <mdb-row>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col col="1"></mdb-col>
            <mdb-col class="text-center">
                <mdb-row>
                    <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
                    <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
                </mdb-row>
                <br />
                <br />
                <!--<ag-grid-vue style="width: 600px; height: 600px;"
                             class="ag-theme-blue mx-auto"
                             :gridOptions="gridOptions"
                             @grid-ready="onGridReady"
                             :columnDefs="columnDefs"
                             :defaultColDef="defaultColDef"
                             :rowData="rowData"
                             :pagination="true">
                </ag-grid-vue>-->
                <v-card>
                    <v-card-title>
                        Login History
                        <v-spacer></v-spacer>
                        <v-text-field v-model="search"
                                      label="Search"
                                      single-line
                                      hide-details></v-text-field>
                    </v-card-title>
                    <v-data-table :headers="columns"
                                  :items="rows"
                                  :search="search"
                                  loading
                                  loading-text="Loading... Please wait"
                                  :sort-by="['DateTime']"
                                  :sort-desc="[false, true]"></v-data-table>
                </v-card>
            </mdb-col>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col col="1"></mdb-col>
        </mdb-row>

        <mdb-row>
            <mdb-col>
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
            </mdb-col>
            <mdb-col>
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
            </mdb-col>
        </mdb-row>

        <button type="button"
                class="bottomLeftButton"
                id="bottomLeftButton"
                onClick="document.getElementById('bottomLeftBanner').remove();this.remove();">
            x
        </button>
        <div class="bottomLeftBanner" id="bottomLeftBanner">
            <square-banner></square-banner>
        </div>

        <button type="button"
                class="bottomRightButton"
                id="bottomRightButton"
                onClick="document.getElementById('bottomRightBanner').remove();this.remove();">
            x
        </button>
        <div class="bottomRightBanner" id="bottomRightBanner">
            <square-banner></square-banner>
        </div>
    </div>
</template>

<script>
    import { mdbRow, mdbCol, Btn } from "mdbvue";

    export default {
        components: { mdbRow, mdbCol, Btn },
        data() {
            return {
                gridOptions: null,
                gridApi: null,
                columnApi: null,
                columnDefs: null,
                defaultColDef: null,
                rowData: null,
                search: null,
                columns: [
                    { text: 'Success', sortable: true, value: 'Success' },
                    { text: 'Date', sortable: true, value: 'DateTime' }
                ],
                rows: []
            };
        },
        methods: {
            onGridReady(params) {
                this.axios
                    .get(process.env.VUE_APP_API_History_Login, {
                        params: { sessionId: this.$cookie.get("sessionId") }
                    })
                    .then(response => {
                        this.rowData = response.data;
                        this.gridApi.sizeColumnsToFit();
                    })
                    .catch(error => {
                        this.$store.commit("errorMessage", error.response.data);
                        this.$store.commit("errorModal", true);
                    });
            }
        },
        beforeMount() {
            this.gridOptions = {};
            this.defaultColDef = { resizable: true, sortable: true, filter: true };
            this.columnDefs = [

                { headerName: 'Success', field: 'Success' },
                { headerName: 'Date', field: 'DateTime', sort: 'desc' }
            ];
        },
        mounted() {
            this.gridApi = this.gridOptions.api;
            this.gridColumnApi = this.gridOptions.columnApi;

            this.axios
                .get(process.env.VUE_APP_API_History_Login, {
                    params: { sessionId: this.$cookie.get("sessionId") }
                })
                .then(response => {
                    this.rows = response.data;
                })
                .catch(error => {
                    this.$store.commit("errorMessage", error.response.data);
                    this.$store.commit("errorModal", true);
                });
        }
    };
</script>